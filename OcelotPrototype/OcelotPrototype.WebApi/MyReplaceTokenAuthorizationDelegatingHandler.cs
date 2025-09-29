using System.Net.Http.Headers;

namespace OcelotPrototype.WebApi
{
    public class MyReplaceTokenAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<MyReplaceTokenAuthorizationDelegatingHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyReplaceTokenAuthorizationDelegatingHandler(
            ILogger<MyReplaceTokenAuthorizationDelegatingHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            AddHeaders(request);
            return await base.SendAsync(request, cancellationToken);
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "123");
            request.Headers.TryAddWithoutValidation("X-TestHeader", "TestValue");
            _logger.LogInformation($"New Token: {request.Headers.Authorization}");
        }
    }
}
