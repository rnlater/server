using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Auth;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public required string Email { get; set; }
}
