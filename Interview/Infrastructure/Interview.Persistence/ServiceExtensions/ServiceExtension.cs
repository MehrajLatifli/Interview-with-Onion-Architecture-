using Interview.Application.Repositories.Custom;
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

            services.AddScoped<IBranchWriteRepository, BranchWriteRepository>();
            services.AddScoped<IBranchReadRepository, BranchReadRepository>();

            services.AddScoped<ICandidateWriteRepository, CandidateWriteRepository>();
            services.AddScoped<ICandidateReadRepository, CandidateReadRepository>();

            services.AddScoped<ICandidateQuestionWriteRepository, CandidateQuestionWriteRepository>();
            services.AddScoped<ICandidateQuestionReadRepository, CandidateQuestionReadRepository>();

            services.AddScoped<ICandidateVacancyWriteRepository, CandidateVacancyWriteRepository>();
            services.AddScoped<ICandidateVacancyReadRepository, CandidateVacancyReadRepository>();

            services.AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();
            services.AddScoped<IDepartmentReadRepository, DepartmentReadRepository>();

            services.AddScoped<IJobDegreeWriteRepository, JobDegreeWriteRepository>();
            services.AddScoped<IJobDegreeReadRepository, JobDegreeReadRepository>();

            services.AddScoped<IOpenQuestionWriteRepository, OpenQuestionWriteRepository>();
            services.AddScoped<IOpenQuestionReadRepository, OpenQuestionReadRepository>();

            services.AddScoped<IQuestionCategoryWriteRepository, QuestionCategoryWriteRepository>();
            services.AddScoped<IQuestionCategoryReadRepository, QuestionCategoryReadRepository>();

            services.AddScoped<IQuestionWriteRepository, QuestionWriteRepository>();
            services.AddScoped<IQuestionReadRepository, QuestionReadRepository>();

            services.AddScoped<IQuestionLevelWriteRepository, QuestionLevelWriteRepository>();
            services.AddScoped<IQuestionLevelReadRepository, QuestionLevelReadRepository>();

            services.AddScoped<IQuestionValueWriteRepository, QuestionValueWriteRepository>();
            services.AddScoped<IQuestionValueReadRepository, QuestionValueReadRepository>();

            services.AddScoped<ISectorWriteRepository, SectorWriteRepository>();
            services.AddScoped<ISectorReadRepository, SectorReadRepository>();

            services.AddScoped<IVacancyWriteRepository, VacancyWriteRepository>();
            services.AddScoped<IVacancyReadRepository, VacancyReadRepository>();


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

