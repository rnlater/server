using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learnings
{
    public class ReviewLearningRequest
    {
        [Required]
        public Guid KnowledgeId;

        [Required]
        public Guid CorrectGameOptionId;

        [Required]
        public Guid GameOptionAnswerId;

        [Required]
        public required string Interpretation;

        [Required]
        public required string WordMatchAnswer;
    }
}