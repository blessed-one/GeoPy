using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UserRegAuthRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [PasswordPropertyText]
    public required string Password { get; set; }
}