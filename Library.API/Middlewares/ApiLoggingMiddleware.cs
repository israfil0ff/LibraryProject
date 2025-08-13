using System.Diagnostics;
using System.Text;

public class SimpleRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SimpleRequestLoggingMiddleware> _logger;

    public SimpleRequestLoggingMiddleware(RequestDelegate next, ILogger<SimpleRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;
        var method = request.Method;
        var path = request.Path;

        
        context.Request.EnableBuffering();

        string requestBody = "";
        if (request.ContentLength > 0 && request.ContentType != null && request.ContentType.Contains("application/json"))
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        await _next(context);

        stopwatch.Stop();

        var responseStatusCode = context.Response.StatusCode;

        
        _logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms\nRequest Body: {RequestBody}",
            method,
            path,
            responseStatusCode,
            stopwatch.ElapsedMilliseconds,
            string.IsNullOrWhiteSpace(requestBody) ? "<empty>" : requestBody);
    }
}
