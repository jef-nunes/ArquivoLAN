using Microsoft.AspNetCore.Http;

namespace Server.Middlewares;

public class MaxRequestBodySizeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly long _maxSizeBytes;

    public MaxRequestBodySizeMiddleware(RequestDelegate next, long maxSizeBytes = 10 * 1024 * 1024)
    {
        _next = next;
        _maxSizeBytes = maxSizeBytes; // 10 MB default
    }

    public async Task Invoke(HttpContext context)
    {
        var contentLength = context.Request.ContentLength;

        // Se o header existir e for maior que o limite
        if (contentLength.HasValue && contentLength > _maxSizeBytes)
        {
            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
            await context.Response.WriteAsync("Payload too large.");
            return;
        }

        await _next(context);
    }
}