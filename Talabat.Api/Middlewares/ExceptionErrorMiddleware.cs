using System.Text.Json;
using Talabat.Api.Errors;

namespace Talabat.Api.Middlewares
{
    public class ExceptionErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionErrorMiddleware> logger;
        private readonly IHostEnvironment environment;

        public ExceptionErrorMiddleware(RequestDelegate next, ILogger<ExceptionErrorMiddleware> logger, IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }catch (Exception e)
            {
                logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var exceptionError = environment.IsDevelopment()
                    ? new ExceptionErrorResponse(StatusCodes.Status500InternalServerError, e.Message, e.StackTrace.ToString())
                    : new ExceptionErrorResponse(StatusCodes.Status500InternalServerError);
                var responseJson = JsonSerializer.Serialize(exceptionError);
                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
