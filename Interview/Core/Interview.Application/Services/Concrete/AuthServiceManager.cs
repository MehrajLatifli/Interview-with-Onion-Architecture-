
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Runtime.ConstrainedExecution;
using System.IO;
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Packaging;
using System.Collections;

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
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _authorizationService;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        public AuthServiceManager(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager, IConfiguration configuration, SignInManager<CustomUser> signInManager, IMapper mapper, ILogger<AuthServiceManager> logger, IAuthorizationService authorizationService, IAuthorizationPolicyProvider policyProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _policyProvider = policyProvider;
        }

        public async Task<List<RoleAccessTypeDTO>> GetRoleAccessType(ClaimsPrincipal User)
        {



            //CustomPolicy.Policy = userRoles.FirstOrDefault();
            //CustomPolicyRole.PolicyRole = userRoles.FirstOrDefault();



            var roleAccessTypes = new List<RoleAccessTypeDTO>();

            var roleAccessType = new RoleAccessTypeDTO();

            // Get all fields of RoleAccessType using reflection
            //var fields = typeof(RoleAccessType).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            //foreach (var field in fields)
            //{
            //    var roleName = field.Name;
            //    var roleValue = (string)field.GetValue(null);

            //    // Assign the value to the first non-null property
            //    if (roleValue != null)
            //    {
            //        roleAccessType.Add ??= roleName == nameof(RoleAccessType.Add_ClaimValue) ? roleValue : null;
            //        roleAccessType.Edit ??= roleName == nameof(RoleAccessType.Edit_ClaimValue) ? roleValue : null;
            //        roleAccessType.Delete ??= roleName == nameof(RoleAccessType.Delete_ClaimValue) ? roleValue : null;
            //        roleAccessType.Get ??= roleName == nameof(RoleAccessType.Get_ClaimValue) ? roleValue : null;
            //        roleAccessType.AllAccess ??= roleName == nameof(RoleAccessType.AllAccess_ClaimValue) ? roleValue : null;
            //    }
            //}





            roleAccessTypes.Add(new RoleAccessTypeDTO
            {
                Id = 1,
                RoleAccess = RoleAccessType.Add_ClaimValue,
            });

            roleAccessTypes.Add(new RoleAccessTypeDTO
            {
                Id = 2,
                RoleAccess = RoleAccessType.Edit_ClaimValue,
            });

            roleAccessTypes.Add(new RoleAccessTypeDTO
            {
                Id = 3,
                RoleAccess = RoleAccessType.Delete_ClaimValue,
            });

            roleAccessTypes.Add(new RoleAccessTypeDTO
            {
                Id = 4,
                RoleAccess = RoleAccessType.Get_ClaimValue,
            });

            roleAccessTypes.Add(new RoleAccessTypeDTO
            {
                Id = 5,
                RoleAccess = RoleAccessType.AllAccess_ClaimValue,
            });


            if (roleAccessTypes.Any())
            {

                return roleAccessTypes;
            }

            else
            {

                throw new NotFoundException("Nothing found!");

            }



        }

        public async Task<List<GetRoleModel>> GetRoles(ClaimsPrincipal User)
        {
           
                //var user_ = User.Identity?.Name;
                //var customuser = await _userManager.FindByNameAsync(user_);

                //var userClaims = await _userManager.GetClaimsAsync(customuser);
                //var userRoles = await _userManager.GetRolesAsync(customuser);
                //var roleClaims = User.FindAll(ClaimTypes.Role);
                //var roles = roleClaims.Select(c => c.Value).ToList();
                //bool isAdmin = roles.Contains(UserRoles.Admin);

                //if (customuser == null)
                //{
                //    throw new NotFoundException("User not found!");
                //}

                var allroles = await _roleManager.Roles.ToListAsync();

                var roleModels = allroles.Select(role => new GetRoleModel
                {   Id=role.Id.ToString(),
                    Rolename = role.Name,
                }).ToList();

                if (roleModels.Any())
                {

                    return roleModels;
                }

                else
                {

                    throw new NotFoundException("Nothing found!");

                }

        
        }

        public async Task<List<GetAuthModel>> GetAdmins(ClaimsPrincipal User, CustomUserClaimResult customUserClaimResult)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_ = User.Identity?.Name;
                var customuser = await _userManager.FindByNameAsync(user_);

                var userClaims = await _userManager.GetClaimsAsync(customuser);
                var userRoles = await _userManager.GetRolesAsync(customuser);
                var roleClaims = User.FindAll(ClaimTypes.Role);
                var roles = roleClaims.Select(c => c.Value).ToList();

                bool isAdmin = roles.Contains(UserRoles.Admin);

                if (customuser == null)
                {
                    throw new NotFoundException("User not found!");
                }

                var ClaimTypeList = userClaims.Select(claim => claim.Type).ToList();
                var ClaimValueList = userClaims.Select(claim => claim.Value).ToList();

                if (customUserClaimResult.UserIds.Contains(customuser.Id))
                {
                    var ClaimTypeIntersect = customUserClaimResult.ClaimTypes.Intersect(ClaimTypeList).ToList();

                    if (ClaimTypeIntersect.Any(i=>i == "Admin"))
                    {
                        var ClaimValueListIntersect = customUserClaimResult.ClaimValues.Intersect(ClaimValueList).ToList();

                        if (ClaimValueListIntersect.Any(i => i == "All Access"))
                        {
                            
                                var list = new List<GetAuthModel>();

                                var usersInHRRole = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);

                                foreach (var item in usersInHRRole)
                                {
                                    var claims = await _userManager.GetClaimsAsync(item);

                                    var userClaims_ = claims.Select(c => new UserClaimModel
                                    {
                                        Role = c.Type,
                                        Permition = c.Value
                                    }).ToList();

                                    list.Add(new GetAuthModel()
                                    {
                                        Id = item.Id.ToString(),
                                        Username = item.UserName,
                                        PhoneNumber = item.PhoneNumber,
                                        Email = item.Email,
                                        ImagePath = item.ImagePath,
                                        UserClaims = userClaims_,
                                    });
                                }

                                if (list.Any())
                                {
                                    return list;
                                }
                                else
                                {
                                    throw new NotFoundException("No users found!");
                                }
                         
                        }
                    }
                }

                throw new ForbiddenException("You do not have permission to access users.");
            }
            else
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
        }

        public async Task<List<GetAuthModel>> GetHR(ClaimsPrincipal User)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_ = User.Identity?.Name;
                var customuser = await _userManager.FindByNameAsync(user_);

                var userClaims = await _userManager.GetClaimsAsync(customuser);
                var userRoles = await _userManager.GetRolesAsync(customuser);
                var roleClaims = User.FindAll(ClaimTypes.Role);
                var roles = roleClaims.Select(c => c.Value).ToList();
            

                if (customuser == null)
                {
                    throw new NotFoundException("User not found!");
                }

              
                    var list = new List<GetAuthModel>();
                    var usersInHRRole = await _userManager.GetUsersInRoleAsync(UserRoles.HR);

                    foreach (var item in usersInHRRole)
                    {
                        var claims = await _userManager.GetClaimsAsync(item);

                        var userClaims_ = claims.Select(c => new UserClaimModel
                        {
                            Role = c.Type,
                            Permition = c.Value
                        }).ToList();


                        list.Add(new GetAuthModel()
                        {
                            Id = item.Id.ToString(),
                            Username = item.UserName,
                            PhoneNumber = item.PhoneNumber,
                            Email = item.Email,
                            ImagePath = item.ImagePath,
                            UserClaims = userClaims_, // Fix here
                        });
                    }

                    if (list.Any())
                    {
                        return list;
                    }
                    else
                    {
                        throw new NotFoundException("No  users found!");
                    }
             
            }
            else
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
        }

        public async Task CreateRole(string roleName, ClaimsPrincipal User)
        {


            if (User.Identity.IsAuthenticated)
            {


                var roleExists = await _roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    var newRole = new CustomRole { Name = roleName };
                    var result = await _roleManager.CreateAsync(newRole);

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to create the role.");
                    }



                    var role = await _roleManager.FindByNameAsync(roleName);

                    if (role != null)
                    {
                        var roleClaims = new List<Claim>
                        {
                            new Claim(roleName, roleName.ToUpper())
                        };

                        foreach (var claim in roleClaims)
                        {
                            await _roleManager.AddClaimAsync(role, claim);
                        }
                    }

                }
                else
                {
                    throw new ConflictException("Role already exists");
                }

            }
            else
            {
                throw new UnauthorizedException("Current user is not authenticated.");
            }



        }

        public async Task AddUserRole(int userId, int roleId, int roleAccessType, ClaimsPrincipal User)
        {
            if (User.Identity.IsAuthenticated)
            {

                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user == null)
                {
                    throw new NotFoundException("User not found!");
                }

                var customRole = await _roleManager.FindByIdAsync(roleId.ToString());


                if (customRole is null)
                {
                    throw new NotFoundException($"There is no role with ID {roleId}.");
                }


                if (!(roleAccessType is int))
                {
                    throw new BadHttpRequestException("RoleAccessType is NOT of type int");
                }

                if (!(roleAccessType is 1) && !(roleAccessType is 2) && !(roleAccessType is 3) && !(roleAccessType is 4) && !(roleAccessType is 5))
                {
                    throw new NotFoundException("RoleAccessType does not exist");
                }


                var isInRole = await _userManager.IsInRoleAsync(user, customRole.Name);

                if (!isInRole)
                {
                    var customUser = await _userManager.FindByIdAsync(userId.ToString());

                    if (customUser != null)
                    {
                        if (customRole != null)
                        {
                            


                            var userClaims = new List<Claim>();

                            switch (roleAccessType)
                            {
                                case 1:
                                    userClaims.Add(new Claim(customRole.Name, RoleAccessType.Add_ClaimValue));
                                    break;
                                case 2:
                                    userClaims.Add(new Claim(customRole.Name, RoleAccessType.Edit_ClaimValue));
                                    break;
                                case 3:
                                    userClaims.Add(new Claim(customRole.Name, RoleAccessType.Delete_ClaimValue));
                                    break;
                                case 4:
                                    userClaims.Add(new Claim(customRole.Name, RoleAccessType.Get_ClaimValue));
                                    break;
                                case 5:
                                    userClaims.Add(new Claim(customRole.Name, RoleAccessType.AllAccess_ClaimValue));
                                    break;
                            }

                            foreach (var claim in userClaims)
                            {
                                await _userManager.AddClaimAsync(user, claim);
                            }


                            await _userManager.AddToRoleAsync(customUser, customRole.Name);
                        }
                    }
                }
                else
                {
                    throw new ConflictException("Role already exists");
                }

            }
            else
            {
                throw new ConflictException("User is already in the specified claim.");
            }
        }

        public async Task UpdateUserRole(int userId, int roleId, int roleAccessType, ClaimsPrincipal User)
        {
            if (User.Identity.IsAuthenticated)
            {

            }
            else
            {
                throw new ConflictException("User is already in the specified claim.");
            }
        }

        public async Task DeleteRole(int roleId, int userId, ClaimsPrincipal User)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (role == null)
            {
                throw new NotFoundException($"Role '{roleId}' not found!");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User '{userId}' not found!");
            }

            //// Delete role claims
            //var roleClaims = await _roleManager.GetClaimsAsync(role);
            //foreach (var claim in roleClaims)
            //{
            //    var result = await _roleManager.RemoveClaimAsync(role, claim);
            //    if (!result.Succeeded)
            //    {
            //        throw new ApplicationException($"Error removing claim '{claim.Type}' from role '{roleId.ToString()}': {string.Join(", ", result.Errors)}");
            //    }
            //}

            // Remove role claims from user identity
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claimToDelete = userClaims.FirstOrDefault(c => c.Type == role.Name);

            if (claimToDelete != null)
            {
                // Remove the specific claim
                var result = await _userManager.RemoveClaimAsync(user, claimToDelete);

                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Error removing claim '{role.Name}' from user '{userId.ToString()}': {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                throw new NotFoundException($"Claim with type '{role.Name}' not found for user '{userId}'.");
            }


            var userroleResult = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (!userroleResult.Succeeded)
            {
                throw new ApplicationException($"Error removing user '{userId}' from role '{role.Name}': {string.Join(", ", userroleResult.Errors)}");
            }

            //// Delete the role
            //var deleteResult = await _roleManager.DeleteAsync(role);
            //if (!deleteResult.Succeeded)
            //{
            //    throw new ApplicationException($"Error deleting role '{roleId.ToString()}': {string.Join(", ", deleteResult.Errors)}");
            //}
        }


        public async Task<LoginResponse> Login(LoginDTO model)
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




                LoginResponseDTO loginResponse = new LoginResponseDTO
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

            Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
            Azure.Storage.Blobs.BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            Azure.Storage.Blobs.BlobClient blobClient = containerClient.GetBlobClient(blobName);

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

            Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
            Azure.Storage.Blobs.BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            Azure.Storage.Blobs.BlobClient blobClient = containerClient.GetBlobClient(blobName);

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

        public async Task RegisterUser(RegisterDTO model, string ConnectionStringAzure)
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

            Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
            Azure.Storage.Blobs.BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            Azure.Storage.Blobs.BlobClient blobClient = containerClient.GetBlobClient(blobName);

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
            };


            var result = await _userManager.CreateAsync(user, entity.Password);

            var cuser = await _userManager.FindByNameAsync(entity.Username);


            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                var errorMessage = string.Join(" ", errors);
                throw new ConflictException("User creation failed.");
            }

          



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

                    Azure.Storage.Blobs.BlobServiceClient blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient(connectionString);
                    Azure.Storage.Blobs.BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                    await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

                    Azure.Storage.Blobs.BlobClient blobClient = containerClient.GetBlobClient(blobName);

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
