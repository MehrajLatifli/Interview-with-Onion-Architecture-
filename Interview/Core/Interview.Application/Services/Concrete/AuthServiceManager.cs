using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Interview.Application.Mapper.AuthDTO;
using Interview.Application.Mapper.DTO;
using Interview.Application.Services.Abstract;
using Interview.Domain.Entities.AuthModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Interview.Application.Exception;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Interview.Domain.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Net;
using Interview.Domain.Entities.IdentityAuth;
using Interview.Application.Repositories.Custom;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Interview.Application.Services.Concrete
{
    public class AuthServiceManager : IAuthService
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<CustomUser> _signInManager;
        public readonly IMapper _mapper;
        readonly ILogger<AuthServiceManager> _logger;

        public AuthServiceManager(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IMapper mapper, ILogger<AuthServiceManager> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetAuthModel>> GetAdmins(ClaimsPrincipal User)
        {

            if (!User.Identity.IsAuthenticated)
            {


                throw new UnauthorizedException("User not authenticated.");

            }

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.Admin))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,

                    });
                }

                else
                {

                    throw new NotFoundException("User not found!");


                }

            }
            if (list.Any())
            {

                return list;
            }

            else
            {


                throw new NotFoundException("Nothing found!");

            }
        }

        public async Task<List<GetAuthModel>> GetHR(ClaimsPrincipal User)
        {
            if (!User.Identity.IsAuthenticated)
            {


                throw new UnauthorizedException("User not authenticated.");



            }

            var list = new List<GetAuthModel>();


            foreach (var user in _userManager.Users.ToList().Where(i => i.Roles == UserRoles.HR))
            {

                if (user != null)
                {


                    list.Add(new GetAuthModel()
                    {
                        Username = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        ImagePath = user.ImagePath,
                        Roles = user.Roles,

                    });
                }

                else
                {



                    throw new NotFoundException("User not found");



                }

            }
            if (list.Any())
            {

                return list;
            }

            else
            {
                throw new NotFoundException("Nothing found!");


                //throw new NotFoundException("Nothing found!");

            }
        }

        public async Task<LoginResponse> Login(LoginDTO model)
        {
            var entity = _mapper.Map<Login>(model);

            var user = await _userManager.FindByNameAsync( entity.Username);
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


               

                LoginResponseDTO loginResponse =new LoginResponseDTO
                {

                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo.ToString(),

                };


                var loginresult = _mapper.Map<LoginResponse>(loginResponse);


                return loginresult;


            }

            else
            {
         
                    throw new UnauthorizedException("Username or password is incorrect.");
          

            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();




        }

        public async Task<TokenModel> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
       
             
                    throw new BadHttpRequestException("Invalid client request");
                
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {

             
                    throw new BadHttpRequestException("Invalid access token or refresh token");
               

            }

            string username = principal.Identity.Name;


            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
      


                    throw new BadHttpRequestException("Invalid access token or refresh token");
              

            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            TokenModel model = new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };

            return model;
        }

        public async Task RegisterAdmin(RegisterDTO model, string ConnectionStringAzure)
        {

            var entity = _mapper.Map<Register>(model);

            var userExists = await _userManager.FindByNameAsync(entity.Username);

            if (userExists != null)
            {
                throw new ConflictException("User already exists!");
            }

            string connectionString = ConnectionStringAzure;

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
                UserName = entity.Username,
                Email = entity.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = entity.PhoneNumber,
                ImagePath = imageUrl,
                Roles = $"{UserRoles.Admin}",
            };

    

            var result = await _userManager.CreateAsync(user, entity.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join(" ", errors);
                throw new ConflictException("User creation failed.");
            }

            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    CustomRole customRole = new()
                    {
                        Name = $"{UserRoles.Admin}",
                    };

                    await _roleManager.CreateAsync(customRole);
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.HR))
                {
                    CustomRole customRole2 = new()
                    {
                        Name = $"{UserRoles.HR}",
                    };

                    await _roleManager.CreateAsync(customRole2);
                }


                if (!await _userManager.IsInRoleAsync(user, UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

                if (!await _userManager.IsInRoleAsync(user, UserRoles.HR))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.HR);
                }
            }

            //var userAccount = new UserAccount()
            //{
            //    UserName = entity.Username,
            //    Email = entity.Email,
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    PhoneNumber = entity.PhoneNumber,
            //    ImagePath = imageUrl,
            //};

            //await _userAccountWriteRepository.AddAsync(userAccount);

            //var userAccountRole = new UserAccountRole()
            //{
            //    Roles = $"{UserRoles.HR}",
            //};

            //await _userAccountRoleWriteRepository.AddAsync(userAccountRole);

            //var userAllData = new UserAllData()
            //{
            //    UserAccountId = _mapper.Map<UserAccountDTO_forGetandGetAll>(await _userAccountReadRepository.GetByIdAsync(userAccount.Id.ToString(), false)).Id,
            //    UserAccountRoleId = _mapper.Map<UserAccountRoleDTO_forGetandGetAll>(await _userAccountRoleReadRepository.GetByIdAsync(userAccountRole.Id.ToString(), false)).Id,
            //};

            //await _userAllDataWriteRepository.AddAsync(userAllData);

            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(DateTime.UtcNow);


        }

        public async Task RegisterHR(RegisterDTO model, string ConnectionStringAzure)
        {
            var entity = _mapper.Map<Register>(model);

            var userExists = await _userManager.FindByNameAsync(entity.Username);

            if (userExists != null)
            {
          
          
                    throw new ConflictException("User already exists!");
               

            }

            string connectionString = ConnectionStringAzure;

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

            CustomRole customRole = new()
            {
                Name = $"{UserRoles.HR}",
            };


            var result = await _userManager.CreateAsync(user, entity.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join($" ", errors);

   

               

                    throw new ConflictException("User creation failed!");
               
            }



            if (!await _roleManager.RoleExistsAsync(UserRoles.HR))
                await _roleManager.CreateAsync(customRole);

            if (await _roleManager.RoleExistsAsync(UserRoles.HR))
                await _userManager.AddToRoleAsync(user, UserRoles.HR);


            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(DateTime.UtcNow);

        }

        public async Task Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
   

              
                    throw new BadHttpRequestException("Invalid user name");
              
            }



            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task UpdatePassword(UpdatePasswordDTO model, ClaimsPrincipal User)
        {
            var entity = _mapper.Map<UpdatePassword>(model);


            if (!User.Identity.IsAuthenticated)
            {
          

                    throw new UnauthorizedException("User not authenticated.");
               
            }



            var username = User.Identity.Name;

            var currentUser = await _userManager.FindByNameAsync(username);

            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);

            if (currentUser != null)
            {
                adminUsers.Any(i => i.UserName == currentUser.UserName);

                if (currentUser.UserName == null)
                {
            
             

                        throw new NotFoundException("User not found!");
                 
                }



                if (!string.IsNullOrEmpty(entity.OldPassword))
                {
                    var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, entity.OldPassword, entity.NewPassword);
                    if (!changePasswordResult.Succeeded)
                    {

                        var errors = changePasswordResult.Errors.Select(e => e.Description);
                        var errorMessage = string.Join($" ", errors);

                

                            throw new InvalidOperationException($"Failed to update password! \n {errorMessage}");
                    

                    }
                }




                var identityResult = await _userManager.UpdateAsync(currentUser);
                if (!identityResult.Succeeded)
                {
             

             

                        throw new InvalidOperationException($"Failed to update user!");
                   

                }

            }
        }

        public async Task UpdateProfile(UpdateProfileDTO model, ClaimsPrincipal User, string ConnectionStringAzure)
        {
            var entity = _mapper.Map<UpdateProfile>(model);
           
            

            if (!User.Identity.IsAuthenticated)
            {
         
             

                    throw new UnauthorizedException($"User not authenticated.");
               
            }



            var username = User.Identity.Name;

            var currentUser = await _userManager.FindByNameAsync(username);

            var adminUsers = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);

            if (currentUser != null)
            {
                adminUsers.Any(i => i.UserName == currentUser.UserName);

                if (currentUser.UserName == null)
                {
               

             

                        throw new NotFoundException($"User not found!");
                  
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

                 
                            throw new InvalidOperationException($"Failed to update password! \n {errorMessage}");
                      
                    }
                }

                if (entity.ImagePath != null)
                {
                    string blobName = currentUser.UserName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(entity.ImagePath.FileName);

                    string connectionString = ConnectionStringAzure;

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
               
                        throw new BadHttpRequestException("User name is already associated with another user!");
                   
                }

                var identityResult = await _userManager.UpdateAsync(currentUser);
                if (!identityResult.Succeeded)
                {

           
                    

                        throw new InvalidOperationException("Failed to update user!");
                  


                }

            }
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
            {
          
                    throw new SecurityTokenException("Invalid token");
            }

            return principal;

        }
    }
}
