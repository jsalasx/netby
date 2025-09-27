using Microsoft.AspNetCore.Mvc;
using MsAuth.Application.UseCase;
using MsAuth.Infrastructure.Mappers;
using Shared.Dto.User;


namespace MsProducts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUseCase _registerUseCase;

    public UserController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUseCase = registerUseCase;
    }

    // 🔹 POST CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequestDto request, CancellationToken ct)
    {
        try
        { 
            var user = UserMapper.ToEntity(request);
            var response = await _registerUseCase.Execute(user, ct);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // 🔹 POST LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        try
        {
            var response = await _loginUseCase.Execute(request, ct);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}