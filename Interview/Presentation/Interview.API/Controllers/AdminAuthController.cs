using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Interview.Application.Mapper.AuthDTO;
using Interview.Domain.Entities.AuthModels;
using Interview.Domain.Entities.Models;
using Interview.Persistence.Contexts.AuthDbContext.IdentityAuth;
using Interview.Persistence.ServiceExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAuthController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;
        public readonly IMapper _mapper;

        public AdminAuthController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegisterDTO model)
        {

            var entity = _mapper.Map<Register>(model);

            var userExists = await _userManager.FindByNameAsync(entity.Username);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });



            string connectionString = ServiceExtension.ConnectionStringAzure;

            string azuriteConnectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AZURE_STORAGE_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(azuriteConnectionString))
            {
                connectionString = azuriteConnectionString;
            }

            string containerName = "profile-images";
         
            string blobName = entity.Username + "_" + Guid.NewGuid().ToString() + Path.GetExtension(entity.ImagePath.FileName); 

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (Stream stream = entity.ImagePath.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            string imageUrl = blobClient.Uri.ToString();

            

            CustomUser user = new()
            {
                Email = entity.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = entity.Username,
                PhoneNumber = entity.PhoneNumber,
                ImagePath = imageUrl,
                Roles = $"{UserRoles.Admin}",
            };

            var result = await _userManager.CreateAsync(user, entity.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join($" ", errors); 

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"User creation failed! \n {errorMessage}" });
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            if (!await _roleManager.RoleExistsAsync(UserRoles.HR))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.HR));

            if (await _roleManager.RoleExistsAsync(UserRoles.HR))
                await _userManager.AddToRoleAsync(user, UserRoles.HR);


            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(DateTime.UtcNow);

          
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }




        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {

            var entity = _mapper.Map<Login>(model);

            var user = await _userManager.FindByNameAsync(entity.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, entity.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);


                System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

                TimeZone localZone = TimeZone.CurrentTimeZone;
                DateTime localTime = localZone.ToLocalTime(DateTime.UtcNow);



                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "Username or password is incorrect." });

            }

        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout([FromBody] Login model)
        {
            await _signInManager.SignOutAsync();

            return Ok(new Response { Status = "Success", Message = "User logout!" });
        }








        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;


            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        [Route("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDTO model)
        {

            var entity = _mapper.Map<UpdateProfile>(model);


            if (!User.Identity.IsAuthenticated)
            {

                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "User not authenticated." });
            }



            var username = User.Identity.Name;

            var currentUser = await _userManager.FindByNameAsync(username);

            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);

            if (currentUser != null)
            {
                adminUsers.Any(i => i.UserName == currentUser.UserName);

                if (currentUser.UserName == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }


                currentUser.UserName = entity.Username;
                currentUser.Email = entity.Email;
                currentUser.PhoneNumber = entity.PhoneNumber;

    

                if (!string.IsNullOrEmpty(entity.OldPassword))
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, entity.OldPassword, entity.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {

                        var errors = changePasswordResult.Errors.Select(e => e.Description);
                        var errorMessage = string.Join($" ", errors);

                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Failed to update password! \n {errorMessage}" });

                    }
                }

                if (entity.ImagePath != null)
                {
                    string blobName = currentUser.UserName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(entity.ImagePath.FileName);

                    string connectionString = ServiceExtension.ConnectionStringAzure;

                    string azuriteConnectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_AZURE_STORAGE_CONNECTION_STRING");
                    if (!string.IsNullOrEmpty(azuriteConnectionString))
                    {
                        connectionString = azuriteConnectionString;
                    }

                    string containerName = "profile-images";

                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                    BlobClient blobClient = containerClient.GetBlobClient(blobName);

                    using (Stream stream = entity.ImagePath.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, true);
                    }

                    string imageUrl = blobClient.Uri.ToString();
                    currentUser.ImagePath = imageUrl;
                }

                var existingUserWithUserName = await _userManager.FindByNameAsync(entity.Username);
                if (existingUserWithUserName != null && existingUserWithUserName.Id != currentUser.Id)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User name is already associated with another user!" });
                }

                var identityResult = await _userManager.UpdateAsync(currentUser);
                if (!identityResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to update user!" });
                }

            }


    

      


            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }



        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromForm] UpdatePasswordDTO model)
        {

            var entity = _mapper.Map<UpdatePassword>(model);


            if (!User.Identity.IsAuthenticated)
            {

                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Error", Message = "User not authenticated." });
            }



            var username = User.Identity.Name;

            var currentUser = await _userManager.FindByNameAsync(username);

            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);

            if (currentUser != null)
            {
                adminUsers.Any(i => i.UserName == currentUser.UserName);

                if (currentUser.UserName == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found!" });
                }



                if (!string.IsNullOrEmpty(entity.OldPassword))
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, entity.OldPassword, entity.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {

                        var errors = changePasswordResult.Errors.Select(e => e.Description);
                        var errorMessage = string.Join($" ", errors);

                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"Failed to update password! \n {errorMessage}" });

                    }
                }

               


                var identityResult = await _userManager.UpdateAsync(currentUser);
                if (!identityResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed to update user!" });
                }

            }







            return Ok(new Response { Status = "Success", Message = "User updated successfully!" });
        }




        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidateIssuer"],
                audience: _configuration["JWT:ValidateAudience"],
                expires: DateTime.Now.AddHours(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
