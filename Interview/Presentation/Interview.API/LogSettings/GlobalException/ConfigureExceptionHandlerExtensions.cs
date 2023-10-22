using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Web.Http;

namespace Interview.API.LogSettings.GlobalException
{
     public static class ConfigureExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler<T>(this IApplicationBuilder app, ILogger<T> logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var statusCode = context.Response.StatusCode;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error, $"An error occurred with status code {statusCode}", statusCode);

                        string title = contextFeature.Error is HttpResponseException ? "HTTP Error" : "Application Error";

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode = statusCode,
                            Message = contextFeature.Error.Message,
                            Title = title,
                        }));
                    }
                });
            });
        }
    
}
    
}
