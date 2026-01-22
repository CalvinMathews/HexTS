using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hexa_CSTS.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // This line attempts to run the rest of the application pipeline.
                await _next(context);
            }
            catch (Exception ex)
            {
                // If any unhandled exception bubbles up, this block will catch it.

                // Log the detailed error for the developers.
                _logger.LogError(ex, "An unhandled exception has occurred.");

                // Call the helper method to create and send a generic response.
                await HandleExceptionAsync(context);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context)
        {
            // Set the response content type to JSON.
            context.Response.ContentType = "application/json";

            // Set the HTTP status code to 500 for Internal Server Error.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create a simple, anonymous object for our error message.
            var errorResponse = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected internal server error occurred. Please contact support."
            };

            // Serialize the object to a JSON string and send it.
            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}