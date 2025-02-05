using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.PivotEntities
{
    public class LearningListKnowledge : PivotEntity
    {
        public Guid LearningListId { get; set; }
        [ForeignKey("LearningListId")]
        public LearningList? LearningList { get; set; }

        public Guid KnowledgeId { get; set; }
        [ForeignKey("KnowledgeId")]
        public Knowledge? Knowledge { get; set; }
    }
}