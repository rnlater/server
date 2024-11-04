using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Utils;

namespace Domain.Entities.SingleIdEntities;

public class Authentication : SingleIdEntity
{
    public required string HashedPassword { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    [MaxLength(6)]
    public string? ConfirmationCode { get; set; }
    public DateTime? ConfirmationCodeExpiryTime { get; set; }

    public required bool IsEmailConfirmed { get; set; }
    public required bool IsActivated { get; set; }

    [ForeignKey("UserId")]
    [NotMapped]
    public User? User { get; set; }

    public required Guid UserId { get; set; }

    public bool VerifyPassword(string password)
    {
        return HashedPassword == PasswordHasher.HashWithSHA256(password);
    }
    public void SetPassword(string password)
    {
        HashedPassword = PasswordHasher.HashWithSHA256(password);
    }
}
