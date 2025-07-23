using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Requests;

public sealed record UserRegistrationRequest
(
    [Required] [EmailAddress] string Email,
    [Required] [PasswordPropertyText] string Password
);