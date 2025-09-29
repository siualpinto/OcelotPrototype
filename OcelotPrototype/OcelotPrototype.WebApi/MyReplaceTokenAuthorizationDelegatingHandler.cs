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
            RedirectToAuthorizationApi(request);
            return await base.SendAsync(request, cancellationToken);
        }

        private void RedirectToAuthorizationApi(HttpRequestMessage request)
        {
            AuthenticationHeaderValue authHeaderValue = request.Headers.Authorization!;
            string? originalToken = authHeaderValue.Parameter;
            if (!string.IsNullOrWhiteSpace(originalToken))
            {

                TokenResult tokenResult = new TokenResult
                {
                    AccessToken = "123"
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.AccessToken);
                _logger.LogInformation($"New Token: {request.Headers.Authorization}");
            }
        }
    }
}
