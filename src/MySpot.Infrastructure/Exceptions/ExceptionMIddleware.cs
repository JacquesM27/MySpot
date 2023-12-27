using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySpot.Core.Exceptions;

namespace MySpot.Infrastructure.Exceptions
{
    internal sealed class ExceptionMIddleware(ILogger<ExceptionMIddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred: {ex.Message}");
                await HandleExceptionAsync(ex, context);
            }
        }

        private static async Task HandleExceptionAsync(Exception exception, HttpContext context)
        {
            var (statusCode, error) = exception switch
            {
                CustomException => (StatusCodes.Status400BadRequest, new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty), exception.Message)),
                _ => (StatusCodes.Status500InternalServerError, new Error("error","There was an error")),
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(error);
        }

        private record Error(string Code, string Reason);
    }
}
