using System.Text.Json.Serialization;

namespace Integrator.Frontend.Services.Login;

public class LoginRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
    [JsonPropertyName("rememberMe")]
    public bool RememberMe { get; set; }
}