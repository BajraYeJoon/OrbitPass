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

public class AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger) : IAuthService
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

        var token = GenerateJwtToken(user);
        logger.LogInformation("User registered successfully with ID: {UserId}", user.Id);

        return new AuthResponse
        {
            Token = token,
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
        var token = GenerateJwtToken(user);
        return new AuthResponse
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.Role.ToString()
        };
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
}