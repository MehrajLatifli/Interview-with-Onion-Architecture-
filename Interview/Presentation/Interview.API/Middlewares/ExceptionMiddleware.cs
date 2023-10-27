using Interview.Application.Exception;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Serilog.Context;
using System.Net;
using System.Text.Json;
using System.Web.Http.Filters;

namespace Interview.API.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {

    
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
           
            _logger = logger;
        }



        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {

                var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;

                LogContext.PushProperty("user_name", username);


                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);

                _logger.LogError(exception, "Error occurred: {ErrorMessage}", exception.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.UnprocessableEntity;

                    if (validationException.Errors.Any())
                        result = JsonSerializer.Serialize(validationException.Errors);
                    break;

                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;

                case ConflictException:
                    code = HttpStatusCode.Conflict;
                    break;

                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    break;

                case BadRequestException:
                    code = HttpStatusCode.BadRequest;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(new { status = code, title = exception.Message });
            }

            await context.Response.WriteAsync(result);
        }
    }


}

