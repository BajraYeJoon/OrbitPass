namespace OrbitPass.Core.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);

public record LoginRequest(
    string Email,
    string Password
);

public record RefreshTokenRequest(
    string RefreshToken
);
public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
