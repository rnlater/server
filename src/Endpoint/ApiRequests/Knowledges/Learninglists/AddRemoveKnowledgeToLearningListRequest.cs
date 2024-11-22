using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learninglists
{
    public class AddRemoveKnowledgeToLearningListRequest
    {
        [Required]
        public Guid LearningListId { get; set; }
        [Required]
        public Guid KnowledgeId { get; set; }
    }
}