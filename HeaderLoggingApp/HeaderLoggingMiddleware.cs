using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HeaderLoggingApp;

public class HeaderLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HeaderLoggingMiddleware> _logger;

    public HeaderLoggingMiddleware(RequestDelegate next, ILogger<HeaderLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var headers = context.Request.Headers.ToArray();
        var builder = new StringBuilder();
        builder.AppendLine($"Begin logging a total of {headers.Length} headers from the request -> {context.Request.Method} {context.Request.GetDisplayUrl()}");
        foreach (var header in headers)
        {
            int index = Array.IndexOf(headers, header);
            builder.AppendLine($"Header #{index}: {header.Key} = {header.Value}");
        }
        builder.AppendLine("--- END of logging ---");
        _logger.LogInformation(builder.ToString());
        await _next(context);
    }
}
