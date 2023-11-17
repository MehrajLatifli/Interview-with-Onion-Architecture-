
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
using Interview.Domain.Entities.IdentityAuth;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq.Dynamic.Core;
using Elfie.Serialization;
using Humanizer.Localisation;

namespace Interview.Application.Services.Concrete
{
    public class AuthServiceManager : IAuthService
    {

        private readonly IConfiguration _configuration;
        public readonly IMapper _mapper;
        readonly ILogger<AuthServiceManager> _logger;
        private readonly IUserWriteRepository _UserWriteRepository;
        private readonly IUserReadRepository _UserReadRepository;


        public AuthServiceManager(IConfiguration configuration, IMapper mapper, ILogger<AuthServiceManager> logger, IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _UserWriteRepository = userWriteRepository;
            _UserReadRepository = userReadRepository;
        }

        public async Task RegisterUser(RegisterDTO model, string ConnectionStringAzure)
        {


            var entity = _mapper.Map<Register>(model);

            var userExists = _UserReadRepository.GetAll(false).Any(i=>i.UserName==model.Username);

            if (userExists ==true)
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


            User user = new()
            {
                UserName = entity.Username,
                Email = entity.Email,
                Password= PasswordComputeHash(model.Password, Environment.GetEnvironmentVariable("Salt")),
                Phonenumber = entity.PhoneNumber,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                ImagePath = imageUrl,
                
            };


            //var result = await _userManager.CreateAsync(user, entity.Password);

            //var cuser = await _userManager.FindByNameAsync(entity.Username);


            await _UserWriteRepository.AddAsync(user);

            await _UserWriteRepository.SaveAsync();

            //if (!result.Succeeded)
            //{
            //    var errors = result.Errors.Select(e => e.Description);
            //    var errorMessage = string.Join(" ", errors);
            //    throw new ConflictException("User creation failed.");
            //}





            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();

            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime localTime = localZone.ToLocalTime(DateTime.UtcNow);


        }


      
        

        public async Task<LoginResponse> Login(LoginDTO model)
        {
          

            var user = _UserReadRepository.GetAll(false).Where(i => i.UserName == model.Username).FirstOrDefault();

            var passwordHash = PasswordComputeHash(model.Password, Environment.GetEnvironmentVariable("Salt"));
       
            if (user.Password != passwordHash)
            {
                throw new UnauthorizedException("Username or password is incorrect.");
            }

            if (user != null)
            {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);


                User user_ = new()
                {
                    Id= user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = user.Password,
                    Phonenumber = user.Phonenumber,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    ImagePath = user.ImagePath,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays),
                };


                _UserWriteRepository.Update(user_);
                await _UserWriteRepository.SaveAsync();


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

        public static string PasswordComputeHash(string password, string pepper)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordSaltPepper = $"{password}{pepper}";
                var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
                var byteHash = sha256.ComputeHash(byteValue);
                var hash = Convert.ToBase64String(byteHash);

                return hash;
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
