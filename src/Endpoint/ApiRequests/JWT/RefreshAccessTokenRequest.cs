using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.JWT;

public class RefreshAccessTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required.")]
    public required string RefreshToken { get; set; }
}
