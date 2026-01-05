using System;
using System.Text.Json;

namespace E_Commerce.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                InvalidIdException => StatusCodes.Status400BadRequest,
                RequestEmptyException => StatusCodes.Status400BadRequest,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                unauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            };

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }

    // DTO للـ Error Response
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // Extension Method
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
}
