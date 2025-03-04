using Microsoft.AspNetCore.Components.Authorization;

namespace Integrator.Frontend.Services.Auth;

public class IntegratorAuthenticationStateProvider : AuthenticationStateProvider
{
    private AuthenticationState _authenticationState;

    public IntegratorAuthenticationStateProvider(AuthenticationService authenticationService)
    {
        _authenticationState = new AuthenticationState(authenticationService.CurrentUser);

        authenticationService.UserChanged += (newUser) =>
        {
            _authenticationState = new AuthenticationState(newUser);
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(newUser)));
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_authenticationState);
    }
}