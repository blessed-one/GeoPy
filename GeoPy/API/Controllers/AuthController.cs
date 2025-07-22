using API.DTOs;
using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromForm] UserRegAuthRequest request)
    {
        try
        {
            var authResult = await authService.LoginAsync(request.Email, request.Password);

            return Ok(authResult);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Неверное имя пользователя или пароль");
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка при авторизации: {errorMessage}", ex.Message);
            return BadRequest();
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromForm] UserRegAuthRequest request)
    {
        try
        {
            await authService.RegisterAsync(request.Email, request.Password);

            return Created();
        }
        catch (ArgumentException argEx)
        {
            return Conflict(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("Ошибка при регистрации: {errorMessage}", ex.Message);
            return BadRequest();
        }
    }
}