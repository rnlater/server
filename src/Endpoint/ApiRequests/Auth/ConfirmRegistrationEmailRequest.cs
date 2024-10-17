using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Auth;

public class ConfirmRegistrationEmailRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Confirmation code must be 6 characters long.")]
    public required string ConfirmationCode { get; set; }
}
