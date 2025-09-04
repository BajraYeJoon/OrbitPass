using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrbitPass.Application.Interfaces;
using OrbitPass.Core.DTOs;
using OrbitPass.Core.Entities;
using Serilog;

namespace OrbitPass.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            logger.LogInformation("Registering user with email: {Email}", request.Email);
            var res = await authService.Register(request);

            logger.LogInformation("User registered successfully: {Email}", request.Email);
            return Ok(res);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning("Registration failed: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user: {Email}", request.Email);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            logger.LogInformation("Login request received for email: {Email}", request.Email);

            var response = await authService.Login(request);

            logger.LogInformation("User logged in successfully: {Email}", request.Email);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning("Login failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during login for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            logger.LogInformation("Refreshing token for refresh token: {RefreshToken}", refreshTokenRequest.RefreshToken);

            var res = await authService.RefreshTokenAsync(refreshTokenRequest);

            logger.LogInformation("Token refreshed successfully for refresh token: {RefreshToken}", refreshTokenRequest.RefreshToken);
            return Ok(res);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning("Token refresh failed: {Message}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during token refresh for refresh token: {RefreshToken}", refreshTokenRequest.RefreshToken);
            return StatusCode(500, new { message = "An error occurred during token refresh" });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(RefreshTokenRequest request)
    {
        try
        {
            logger.LogInformation("Logging out user with refresh token: {RefreshToken}", request.RefreshToken);

            await authService.LogoutAsync(request);

            return Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during logout for refresh token: {RefreshToken}", request.RefreshToken);
            return StatusCode(500, new { message = "An error occurred during logout" });
        }
    }


}