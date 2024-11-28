using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> options)
    {
        this._jwtOptions = options.Value;
    }

    public string GenerateToken(string userId, string role, string accountType)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new("userId", userId),
            new("role", role),
            new("accountType", accountType)
        ];

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: this._jwtOptions.Issuer,
            audience: this._jwtOptions.Audience,
            expires: DateTime.Now.AddHours(this._jwtOptions.ExpiresHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}