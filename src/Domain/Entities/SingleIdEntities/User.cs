using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

namespace Domain.Entities.SingleIdEntities
{
    public class User : SingleIdEntity
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }

        [AllowNull]
        public string? PhotoUrl { get; set; }

        public Role Role { get; set; } = Role.User;

        [NotMapped]
        [InverseProperty("User")]
        public Authentication? Authentication { get; set; }
    }
}