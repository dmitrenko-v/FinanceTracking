using Application;
using Application.Dto.Auth;
using Application.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        this._authService = authService;
    }

    [HttpGet("check")]
    public ActionResult Check()
    {
        return this.Ok(User.Claims.Select(x => new { x.Type, x.Value }).ToList());
    }

    [HttpPost("google")]
    public async Task<ActionResult> GoogleAuth(string code)
    {
        var token = await this._authService.GoogleAuth(code);
        this.SetAccessTokenCookie(token);
        return this.Ok();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var token = await this._authService.CredentialsLoginAsync(loginDto);
        this.SetAccessTokenCookie(token);
        return this.Ok();
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var token = await this._authService.CredentialsRegisterAsync(registerDto);
        this.SetAccessTokenCookie(token);
        return this.Ok();
    }
    
    private void SetAccessTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = false,
            Domain = Request.PathBase.Value,
            Path = "/",
        };
        Response.Cookies.Append("accessToken", token, cookieOptions);
    }
}