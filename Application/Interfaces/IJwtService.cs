namespace Application.Interfaces;

public interface IJwtService
{
    public string GenerateToken(string userId, string role, string accountType);
}