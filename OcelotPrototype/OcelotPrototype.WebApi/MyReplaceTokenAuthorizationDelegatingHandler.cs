using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Headers;
using System.Text;

namespace OcelotPrototype.WebApi;

public class MyReplaceTokenAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<MyReplaceTokenAuthorizationDelegatingHandler> _logger;
    private readonly IHttpContextAccessor _contextAccessor;

    public MyReplaceTokenAuthorizationDelegatingHandler(
        ILogger<MyReplaceTokenAuthorizationDelegatingHandler> logger,
        IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
       HttpRequestMessage request,
       CancellationToken cancellationToken)
    {
        AddHeaders(request);
        return await base.SendAsync(request, cancellationToken);
    }

    private void AddHeaders(HttpRequestMessage downstream)
    {
        var upstream = _contextAccessor.HttpContext!.Request;
        var headers = downstream.Headers;
        var builder = new StringBuilder();
        builder.AppendLine($"Incoming (upstream) URL -> {upstream.Method} {upstream.GetDisplayUrl()}");
        builder.AppendLine($"Forwarded to (downstream) URL -> {downstream.Method} {downstream.RequestUri}");

        headers.Authorization = new AuthenticationHeaderValue("Bearer", "123");
        headers.TryAddWithoutValidation("X-TestHeader", "TestValue");
        builder.AppendLine($"Header {nameof(headers.Authorization)} = {headers.Authorization}");
        builder.AppendLine($"Header X-TestHeader = {string.Join(';', headers.GetValues("X-TestHeader"))}");
        _logger.LogInformation(builder.ToString());
    }
}
