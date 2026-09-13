using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TripCraft.Application.Auth;
using TripCraft.Infrastructure.Identity;

namespace TripCraft.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public record RegisterRequest(string Email, string Password, string DisplayName);
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string AccessToken, string RefreshToken);

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var user = new AppUser { UserName = request.Email, Email = request.Email, DisplayName = request.DisplayName };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => e.Description));

        var access = _tokenService.GenerateAccessToken(user.Id, user.Email!);
        var refresh = _tokenService.GenerateRefreshToken();
        // persist hashed refresh token 
        return Ok(new AuthResponse(access, refresh));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized("Invalid email or password.");

        var access = _tokenService.GenerateAccessToken(user.Id, user.Email!);
        var refresh = _tokenService.GenerateRefreshToken();
        return Ok(new AuthResponse(access, refresh));
    }
}