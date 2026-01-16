using System.Diagnostics;
using System.Text.Json;

namespace LexisNexis.API.Middleware
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestContextMiddleware> _logger;
        private const string RequestIdHeader = "X-Request-Id";

        public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate or retrieve request ID
            string requestId = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..8]}";

            // Add request ID to response headers
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(RequestIdHeader))
                {
                    context.Response.Headers.TryAdd(RequestIdHeader, requestId);
                }
                return Task.CompletedTask;
            });

            // Set Activity ID for logging correlation
            Activity.Current?.SetTag("RequestId", requestId);

            try
            {
                // Proceed with next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log full exception internally
                _logger.LogError(ex, "Unhandled exception occurred (RequestId: {RequestId})", requestId);

                // Return generic error to client
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    RequestId = requestId,
                    Message = "An internal server error occurred."
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
