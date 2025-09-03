using Microsoft.AspNetCore.Mvc;
using OrbitPass.Application.Interfaces;
using OrbitPass.Core.DTOs;
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
}