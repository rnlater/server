using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Confirm Password must be equal to Password.")]
    public required string ConfirmationPassword { get; set; }
}
