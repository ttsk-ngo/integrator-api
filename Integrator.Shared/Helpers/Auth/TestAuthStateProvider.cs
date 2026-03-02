using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class TestAuthStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _testUser;

    public TestAuthStateProvider(string[] roles, string username = "TestUser")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, "TestUserId123"),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, "TestAuth");

        _testUser = new ClaimsPrincipal(identity);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(_testUser);
        return Task.FromResult(state);
    }
}
