using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;
using TheOneCRM.API.Error;
using TheOneCRM.Application.DTOs;
namespace TheOneCRM.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Cannot handle exception because the response has already started.");
                    LogError(context, ex, stopwatch.Elapsed.TotalMilliseconds);
                    throw;
                }

                await HandleExceptionAsync(context, ex, stopwatch.Elapsed.TotalMilliseconds);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, double elapsedMilliseconds)
        {
            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized access."
                ),

                SecurityTokenException => (
                    StatusCodes.Status401Unauthorized,
                    "Invalid or expired token."
                ),

                BadHttpRequestException => (
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ),

                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    ex.Message
                ),

                DbUpdateConcurrencyException => (
                    StatusCodes.Status409Conflict,
                    "The resource was modified by another process. Please refresh and try again."
                ),

                DbUpdateException dbUpdateEx => MapDbUpdateException(dbUpdateEx),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later."
                )
            };

            var details = GetDetailedMessage(ex);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            ApiErrorResponse response;

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                response = _env.IsDevelopment()
                    ? new ApiErrorResponse(statusCode, message, details)
                    : new ApiErrorResponse(statusCode, message);
            }
            else
            {
                response = new ApiErrorResponse(statusCode, message, details);
            }

            await WriteJsonResponse(context, response);

            LogError(context, ex, elapsedMilliseconds);
        }

        private async Task WriteJsonResponse(HttpContext context, ApiErrorResponse response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private void LogError(HttpContext context, Exception ex, double elapsedMilliseconds)
        {
            using (LogContext.PushProperty("RequestPath", context.Request.Path.ToString()))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
            using (LogContext.PushProperty("ElapsedMilliseconds", elapsedMilliseconds))
            using (LogContext.PushProperty("User", context.User?.Identity?.Name ?? "Anonymous"))
            using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
            using (LogContext.PushProperty("SpanId", Activity.Current?.SpanId.ToString() ?? "N/A"))
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
            }
        }

        private static (int statusCode, string message) MapDbUpdateException(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlException)
            {
                return sqlException.Number switch
                {
                    547 => (
                        StatusCodes.Status409Conflict,
                        "Operation failed بسبب ارتباط البيانات (Foreign Key constraint)."
                    ),
                    2601 or 2627 => (
                        StatusCodes.Status409Conflict,
                        "Operation failed بسبب تكرار قيمة يجب أن تكون فريدة (Unique constraint)."
                    ),
                    _ => (
                        StatusCodes.Status400BadRequest,
                        "Database update failed. Please check the submitted data."
                    )
                };
            }

            return (
                StatusCodes.Status400BadRequest,
                "Database update failed. Please check the submitted data."
            );
        }

        private static string GetDetailedMessage(Exception ex)
        {
            var messages = new List<string>();
            Exception? current = ex;

            while (current != null)
            {
                if (!string.IsNullOrWhiteSpace(current.Message))
                    messages.Add(current.Message);

                current = current.InnerException;
            }

            return string.Join(" | ", messages.Distinct());
        }
    }
}


