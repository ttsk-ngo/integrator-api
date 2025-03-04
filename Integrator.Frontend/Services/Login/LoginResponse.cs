using System.Text.Json.Serialization;

namespace Integrator.Frontend.Services.Login;

public class LoginResponse
{
    [JsonPropertyName("tokenType")]
    public string TokenType { get; set; }
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expiresIn")]
    public ulong ExpiresIn { get; set; }
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}