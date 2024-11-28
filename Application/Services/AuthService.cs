using System.Net.Http.Json;
using System.Text.Json;
using Application.Dto.Auth;
using Application.Identity;
using Application.Interfaces;
using Application.Options;
using AutoMapper;
using Domain.Exceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly GoogleOptions _googleOptions;
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AuthService(
        IOptions<GoogleOptions> googleOptions,
        UserManager<User> userManager,
        IJwtService jwtService,
        IMapper mapper
        )
    {
        this._googleOptions = googleOptions.Value;
        this._userManager = userManager;
        this._jwtService = jwtService;
        this._mapper = mapper;
    }

    public async Task<string> GoogleAuth(string code)
    {
        var idToken = await this.GetIdTokenAsync(code);
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken,
            new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [this._googleOptions.ClientId]
            });

        var userId = payload.Name!;

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            var newUser = new User
            {
                Id = userId,
                Email = payload.Email,
                UserName = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                SecurityStamp = Guid.NewGuid().ToString(),
                AccountTypeName = "Base",
            };

            var result = await this._userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                throw new BadRequestException(JsonSerializer.Serialize(result.Errors));
            }
            await this._userManager.AddToRoleAsync(newUser, "User");
            return this._jwtService.GenerateToken(newUser.Id, "User", newUser.AccountTypeName);
        }
        var role = (await this._userManager.GetRolesAsync(user))[0];
        return this._jwtService.GenerateToken(user.Id, role, user.AccountTypeName);
    }

    public async Task<string> CredentialsLoginAsync(LoginDto loginDto)
    {
        var user = await this._userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
        {
            throw new BadRequestException("There is no user with given email");
        }
        
        var result = await this._userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result)
        {
            throw new BadRequestException("Invalid password");
        }
        
        var role = (await this._userManager.GetRolesAsync(user))[0];
        return this._jwtService.GenerateToken(user.Id, role, user.AccountTypeName);
    }

    public async Task<string> CredentialsRegisterAsync(RegisterDto registerDto)
    {

        if (await this._userManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            throw new BadRequestException("User with given email already exists.");
        }
        
        var applicationUser = this._mapper.Map<User>(registerDto);
        
        applicationUser.Id = Guid.NewGuid().ToString();
        applicationUser.UserName = registerDto.Email;
        applicationUser.SecurityStamp = Guid.NewGuid().ToString();
        applicationUser.AccountTypeName = "Base";

        var result = await this._userManager.CreateAsync(applicationUser, registerDto.Password);

        if (result.Succeeded)
        {
            await this._userManager.AddToRoleAsync(applicationUser, "User");
            return this._jwtService.GenerateToken(applicationUser.Id, "User", applicationUser.AccountTypeName);
        }

        throw new BadRequestException(JsonSerializer.Serialize(result.Errors));

    }

    private async Task<string> GetIdTokenAsync(string code)
    {
        using var client = new HttpClient();
        var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", this._googleOptions.ClientId },
                { "client_secret", this._googleOptions.ClientSecret },
                { "redirect_uri", "http://localhost:4200/signin-google" },
                { "grant_type", "authorization_code" }
            }));

        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new BadRequestException("Authentication failed. Please, try again");
        }

        var tokenData = await tokenResponse.Content.ReadFromJsonAsync<GoogleResponse>();
        if (tokenData is null)
        {
            throw new BadRequestException("Failed to retrieve google account info");
        }

        return tokenData.IdToken;
    }
}