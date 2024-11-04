namespace Application.DTOs;

public class AuthenticationDto : SingleIdEntityDto
{
    public required string HashedPassword { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public string? ConfirmationCode { get; set; }
    public DateTime? ConfirmationCodeExpiryTime { get; set; }

    public required bool IsEmailConfirmed { get; set; }
    public required bool IsActivated { get; set; }

    public UserDto? User { get; set; }

    public required Guid UserId { get; set; }
}
