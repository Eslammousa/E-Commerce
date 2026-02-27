using E_Commerce.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                    throw;

                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. TraceId: {TraceId}",
                    context.TraceIdentifier);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                // 400 - Bad Request
                InvalidIdException => StatusCodes.Status400BadRequest,
                RequestEmptyException => StatusCodes.Status400BadRequest,
                InvalidQuantityException => StatusCodes.Status400BadRequest,

                // 409 - Conflict
                InsufficientStockException => StatusCodes.Status409Conflict,
                DuplicateEntityException => StatusCodes.Status409Conflict,

                // 404 - Not Found
                EntityNotFoundException => StatusCodes.Status404NotFound,

                // 401 - Unauthorized
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

                // 415 - Unsupported Media Type
                InvalidImageTypeException => StatusCodes.Status415UnsupportedMediaType,

                // 413 - Payload Too Large
                ImageSizeExceededException => StatusCodes.Status413PayloadTooLarge,

                // 500 - Internal Server Error
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = context.Response.StatusCode == StatusCodes.Status500InternalServerError
                    ? "An unexpected error occurred"
                    : ex.Message,
                TraceId = context.TraceIdentifier
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    // Error Response DTO
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? TraceId { get; set; }
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
