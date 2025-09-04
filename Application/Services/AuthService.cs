using AutoMapper;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using OrbitPass.Application.Interfaces;
using OrbitPass.Core.DTOs;
using OrbitPass.Core.Entities;
using OrbitPass.Core.Enums;
using OrbitPass.Core.Interfaces;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OrbitPass.Application.Services;

public class AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        //check if user exists
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser is not null) throw new InvalidOperationException("User already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = mapper.Map<User>(request);

        user.PasswordHash = passwordHash;
        user.Role = UserRole.Attendee;

        logger.LogInformation("Creating user {@User}", user);
        await userRepository.CreateAsync(user);

        var accessToken = GenerateJwtToken(user);
        var refreshTokenValue = GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,

        };

        await refreshTokenRepository.CreateAsync(refreshToken);

        logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.Role.ToString()
        };
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        // 1. Find user by email
        var user = await userRepository.GetByEmailAsync(request.Email) ?? throw new UnauthorizedAccessException("Invalid email or password");

        // 2. Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        // 3. Generate JWT token
        var accessToken = GenerateJwtToken(user);
        var refreshTokenValue = GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id,

        };

        await refreshTokenRepository.CreateAsync(refreshToken);


        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.Role.ToString()
        };
    }


    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        logger.LogInformation("Refreshing token for refresh token: {RefreshToken}", request.RefreshToken);

        var existingTokenFromDB = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (existingTokenFromDB is null)
        {
            logger.LogWarning("Refresh token not found or revoked: {RefreshToken}", request.RefreshToken);
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        if (existingTokenFromDB.ExpiresAt < DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token expired: {RefreshToken}", request.RefreshToken);
            throw new UnauthorizedAccessException("Refresh token has expired");
        }

        var accessToken = GenerateJwtToken(existingTokenFromDB.User);
        logger.LogInformation("Generated new access token for user ID: {UserId}", existingTokenFromDB.UserId);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = request.RefreshToken,
            Email = existingTokenFromDB.User.Email,
            FirstName = existingTokenFromDB.User.FirstName,
            Role = existingTokenFromDB.User.Role.ToString()
        };
    }

    public async Task LogoutAsync(RefreshTokenRequest request)
    {
        var existingRefreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (existingRefreshToken is not null)
        {
            existingRefreshToken.IsRevoked = true;
            await refreshTokenRepository.UpdateAsync(existingRefreshToken);

            logger.LogInformation("Refresh token revoked successfully: {RefreshToken}", request.RefreshToken);
        }
        else
        {
            logger.LogWarning("Attempted to revoke non-existent refresh token: {RefreshToken}", request.RefreshToken);
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = configuration["AppSettings:Issuer"],
            Audience = configuration["AppSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}