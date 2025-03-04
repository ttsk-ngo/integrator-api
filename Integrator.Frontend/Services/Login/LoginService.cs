using System.Security.Claims;
using Integrator.Frontend.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using RestSharp;

namespace Integrator.Frontend.Services.Login;

public interface ILoginService
{
    Task<bool> Login(LoginRequest loginRequest);
}

public class LoginService : ILoginService
{
    private readonly RestClient _restClient;
    private readonly AuthenticationService _authenticationService;
    private readonly string _integratorApiHost;

    public LoginService(RestClient restClient, IConfiguration configuration, AuthenticationService authenticationService)
    {
        _restClient = restClient;
        _authenticationService = authenticationService;
        _integratorApiHost = configuration.GetSection("IntegratorApiHost").Value!;
    }
    
    public async Task<bool> Login(LoginRequest loginRequest)
    {
        var restRequest = new RestRequest($"{_integratorApiHost}/account/login", Method.Post);
        restRequest.AddQueryParameter("useCookies", "false");
        restRequest.AddQueryParameter("useSessionCookies", loginRequest.RememberMe ? "false" : "true");
        restRequest.AddJsonBody(loginRequest);
        
        var response = await _restClient.ExecuteAsync<LoginResponse>(restRequest);

        if (response.IsSuccessful && response.Data != null)
        {
            _restClient.AddDefaultHeader(KnownHeaders.Authorization, $"Bearer {response.Data.AccessToken}");
            
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, loginRequest.Email) }, "IntegratorLocalAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _authenticationService.CurrentUser = claimsPrincipal;
        }

        return response.IsSuccessful;
    }
}