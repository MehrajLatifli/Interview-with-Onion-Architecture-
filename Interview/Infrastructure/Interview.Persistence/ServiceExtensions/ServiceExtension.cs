
using Interview.Application.Repositories.Custom;
using Interview.Application.Services.Abstract;
using Interview.Application.Services.Concrete;
using Interview.Persistence.Contexts.AuthDbContext.DbContext;
using Interview.Persistence.Contexts.InterviewDbContext;
using Interview.Persistence.Repositories.Custom;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Interview.Domain.Entities.AuthModels;

namespace Interview.Persistence.ServiceExtensions
{
    public static class ServiceExtension
    {
        public static string CustomDbConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Interview.API"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("CustomDbConnection");
            }
        }

        public static string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Interview.API"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("DefaultConnection");
            }
        }


        public static string ConnectionStringAzure
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Interview.API"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager["ConnectionAzureStorage"];
            }
        }


        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<InterviewContext>(options => options.UseSqlServer(ConnectionString));




            services.AddScoped<IAuthService, AuthServiceManager>();

            services.AddScoped<ICandidateDocumentService, CandidateDocumentServiceManager>();

            services.AddScoped<ICandidateService, CandidateServiceManager>();

            services.AddScoped<ICategoryService, CategoryServiceManager>();

            services.AddScoped<ILevelService, LevelServiceManager>();

            services.AddScoped<IPositionService, PositionServiceManager>();

            services.AddScoped<IQuestionService, QuestionServiceManager>();

            services.AddScoped<ISessionQuestionService, SessionQuestionServiceManager>();

            services.AddScoped<ISessionService, SessionServiceManager>();

            services.AddScoped<IStructureService, StructureServiceManager>();

            services.AddScoped<IStructureTypeService, StructureTypeServiceManager>();

            services.AddScoped<IVacancyService, VacancyServiceManager>();




            services.AddScoped<ICandidateDocumentWriteRepository, CandidateDocumentWriteRepository>();
            services.AddScoped<ICandidateDocumentReadRepository, CandidateDocumentReadRepository>();

            services.AddScoped<ICandidateWriteRepository, CandidateWriteRepository>();
            services.AddScoped<ICandidateReadRepository, CandidateReadRepository>();

            services.AddScoped<IPositionWriteRepository, PositionWriteRepository>();
            services.AddScoped<IPositionReadRepository, PositionReadRepository>();

            services.AddScoped<IVacancyWriteRepository, VacancyWriteRepository>();
            services.AddScoped<IVacancyReadRepository, VacancyReadRepository>();

            services.AddScoped<IStructureTypeWriteRepository, StructureTypeWriteRepository>();
            services.AddScoped<IStructureTypeReadRepository, StructureTypeReadRepository>();

            services.AddScoped<IStructureWriteRepository, StructureWriteRepository>();
            services.AddScoped<IStructureReadRepository, StructureReadRepository>();

            services.AddScoped<ISessionWriteRepository, SessionWriteRepository>();
            services.AddScoped<ISessionReadRepository, SessionReadRepository>();

            services.AddScoped<ISessionQuestionWriteRepository, SessionQuestionWriteRepository>();
            services.AddScoped<ISessionQuestionReadRepository, SessionQuestionReadRepository>();

            services.AddScoped<IQuestionWriteRepository, QuestionWriteRepository>();
            services.AddScoped<IQuestionReadRepository, QuestionReadRepository>();

            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();

            services.AddScoped<ILevelWriteRepository, LevelWriteRepository>();
            services.AddScoped<ILevelReadRepository, LevelReadRepository>();



        }


        public static IServiceCollection AddSwaggerGenServiceExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Interview WebAPI",
                    Version = "v1",
                    Description = "It is intended for the convenience of the interview process."
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter `Bearer` [space] and then your valid token in the text input below. \r\n\r\n Example: \"Bearer apikey \""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, new string[]{}
                }
        });
            });

            return services;
        }




        public static void AddCustomAuthorizationPoliciesServiceExtension(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole(UserRoles.Admin));

                options.AddPolicy("HROnly", policy =>
                    policy.RequireRole(UserRoles.HR));

                options.AddPolicy("AllRoles", policy =>
                {
                    policy.RequireRole(UserRoles.Admin);
                    policy.RequireRole(UserRoles.HR);
                });
            });
        }



        public static void AddRateLimiterServiceExtension(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {

                    if (httpContext.Request.Path.StartsWithSegments("/api/Auth/login"))
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), partition =>
                            new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 1,
                                AutoReplenishment = true,
                                Window = TimeSpan.FromSeconds(1)
                            });
                    }



                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: "default", partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = int.MaxValue,
                            AutoReplenishment = true,
                            Window = TimeSpan.FromSeconds(1)
                        });
                });

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later... ", cancellationToken: token);
                };
            });
        }
    }
}

