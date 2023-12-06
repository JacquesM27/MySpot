﻿using Humanizer;
using Microsoft.AspNetCore.Http;
using MySpot.Core.Exceptions;

namespace MySpot.Infrastructure.Exceptions
{
    internal sealed class ExceptionMIddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
