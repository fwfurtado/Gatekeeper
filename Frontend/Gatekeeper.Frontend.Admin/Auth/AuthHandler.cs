using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Gatekeeper.Frontend.Admin.Auth;

public class AuthHandler : DelegatingHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuthHandler> _logger; 

    public AuthHandler(IServiceScopeFactory scopeFactory, ILogger<AuthHandler> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var scope =_scopeFactory.CreateScope();
        var provider = scope.ServiceProvider;
        var tokenProvider = provider.GetRequiredService<IAccessTokenProvider>();
        
        var token = await tokenProvider.RequestAccessToken();

        var success = token.TryGetToken(out var accessToken);

        if (success)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
        } else
        {
            _logger.LogError("Failed to get access token");
        }
        
        var response = await base.SendAsync(request, cancellationToken);
        
        return response;
    }
}