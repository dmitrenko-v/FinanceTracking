using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
}