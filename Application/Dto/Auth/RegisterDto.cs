using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Auth;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    
    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    [MaxLength(50, ErrorMessage = "First name must be less than 50 characters long")]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters long")]
    public string LastName { get; set; } = null!;
}