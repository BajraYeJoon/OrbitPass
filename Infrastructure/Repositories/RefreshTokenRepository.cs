using Microsoft.EntityFrameworkCore;
using OrbitPass.Core.Entities;
using OrbitPass.Core.Interfaces;
using OrbitPass.Infrastructure.Data;

namespace OrbitPass.Infrastructure.Repositories;

public class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken> UpdateAsync(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Update(refreshToken);
        await dbContext.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
    }

    //for force logout or end all sessions]
    public async Task RevokeAllUserTokensAsync(int userId)
    {
        var tokens = await dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }

        await dbContext.SaveChangesAsync();
    }
}