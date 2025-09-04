using OrbitPass.Core.Entities;

namespace OrbitPass.Core.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<RefreshToken> UpdateAsync(RefreshToken refreshToken);
    Task RevokeAllUserTokensAsync(int userId);
}