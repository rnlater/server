using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learninglists
{
    public class AddRemoveKnowledgesToLearningListRequest
    {
        [Required]
        public Guid LearningListId { get; set; }
        [Required, MinLength(1)]
        public required List<Guid> KnowledgeIds { get; set; }
        [Required]
        public required bool IsAdd { get; set; }

    }
}