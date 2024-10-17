using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Auth;

public class ConfirmPasswordResettingEmailRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Confirmation code must be 6 characters long.")]
    public required string ConfirmationCode { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Confirm Password must be equal to Password.")]
    public required string ConfirmationPassword { get; set; }
}
