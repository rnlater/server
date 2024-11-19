using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learnings
{
    public class GetLearningsToReviewRequest
    {
        [MaxLength(200)]
        [MinLength(1)]
        public List<Guid> KnowledgeIds { get; set; } = [];
    }
}