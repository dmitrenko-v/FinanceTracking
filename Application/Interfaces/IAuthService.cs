using Application.Dto.Auth;

namespace Application.Interfaces;

public interface IAuthService
{
    public Task<string> GoogleAuth(string code);
    
    public Task<string> CredentialsLoginAsync(LoginDto loginDto);
    
    public Task<string> CredentialsRegisterAsync(RegisterDto registerDto);
}