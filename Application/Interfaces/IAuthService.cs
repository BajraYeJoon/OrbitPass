using OrbitPass.Core.DTOs;

namespace OrbitPass.Application.Interfaces;

public interface IAuthService
{
   Task<AuthResponse> Register(RegisterRequest request);
   Task<AuthResponse> Login(LoginRequest request);
   Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);

   Task LogoutAsync(RefreshTokenRequest request);
}