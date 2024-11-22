using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;

namespace Domain.Entities.SingleIdEntities
{
    public class User : SingleIdEntity
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }

        public string? PhotoUrl { get; set; }

        public Role Role { get; set; } = Role.User;

        public Authentication? Authentication { get; set; }

        public ICollection<Learning> Learnings { get; set; } = [];

        public ICollection<LearningList> LearningLists { get; set; } = [];
    }
}