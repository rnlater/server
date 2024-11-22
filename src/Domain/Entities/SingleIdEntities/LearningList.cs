using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.PivotEntities;

namespace Domain.Entities.SingleIdEntities
{
    public class LearningList : SingleIdEntity
    {
        public Guid LearnerId { get; set; }
        [ForeignKey("LearnerId")]
        public User? User { get; set; }

        public required string Title { get; set; }

        public ICollection<LearningListKnowledge> LearningListKnowledges { get; set; } = [];
    }
}