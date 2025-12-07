using BookHub.Core.Exceptions;
using BookHub.Core.Responses;
using System.Net;
using System.Text.Json;

namespace BookHub.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                NotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                UnauthorizedException => ((int)HttpStatusCode.Unauthorized, exception.Message),
                ForbiddenException => ((int)HttpStatusCode.Forbidden, exception.Message),
                BadRequestException => ((int)HttpStatusCode.BadRequest, exception.Message),
                _ => ((int)HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.")
            };

            context.Response.StatusCode = statusCode;

            var errorResponse = ApiResponse<string>.Fail(message);

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })
            );
        }
    }
}
