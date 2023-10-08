
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

            services.AddScoped<IService, ServiceManager>();

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

            services.AddScoped<ISessionTypeWriteRepository, SessionTypeWriteRepository>();
            services.AddScoped<ISessionTypeReadRepository, SessionTypeReadRepository>();

            services.AddScoped<ILevelWriteRepository, LevelWriteRepository>();
            services.AddScoped<ILevelReadRepository, LevelReadRepository>();



        }

            public static void AddRateLimiterServiceExtension(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {

                    if (httpContext.Request.Path.StartsWithSegments("/api/Authenticate/login"))
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

