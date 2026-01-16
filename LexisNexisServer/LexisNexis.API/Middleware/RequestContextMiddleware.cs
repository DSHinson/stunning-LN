using System.Diagnostics;

namespace LexisNexis.API.Middleware
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;
        private const string RequestIdHeader = "X-Request-Id";

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate or retrieve request ID
            string requestId = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..8]}";

            // Add request ID to response headers for traceability
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(RequestIdHeader))
                {
                    context.Response.Headers.TryAdd(RequestIdHeader, requestId);
                }
                return Task.CompletedTask;
            });

            // Set Activity ID for logging correlation (optional but useful)
            Activity.Current?.SetTag("RequestId", requestId);
            // Call the next middleware in the pipeline
            await _next(context);

        }
    }
}
