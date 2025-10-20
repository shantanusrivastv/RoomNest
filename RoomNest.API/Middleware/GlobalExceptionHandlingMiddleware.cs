using System.Text.Json;

namespace RoomNest.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            string errorMessage = String.Empty;
            switch (exception)
            {
                case InvalidOperationException invalidOpEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.StatusCode = 400;
                    errorMessage = invalidOpEx.Message;
                    break;

                case ArgumentNullException argNullEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    errorMessage = $"Required parameter is missing: {argNullEx.ParamName}";
                    break;

                case ArgumentException argEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    errorMessage = argEx.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.StatusCode = 500;
                    errorMessage = "An unexpected error occurred. Please contact support.";
                    break;
            }

            var errorResponse = new ErrorResponse
            {
                status = context.Response.StatusCode,
                error = errorMessage
            };

            var payload = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(payload);
        }
    }
}