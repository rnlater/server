using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learnings
{
    public class ReviewLearningRequests
    {
        [MaxLength(10)]
        public List<ReviewLearningRequest> GroupedKnowledges { get; set; } = [];
    }

    public class ReviewLearningRequest
    {
        [Required]
        public Guid KnowledgeId { get; set; }

        [Required]
        public Guid CorrectGameOptionId { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public required string Answer { get; set; }

        [Required]
        public required string Interpretation { get; set; }

        [Required]
        public required string WordMatchAnswer { get; set; }
    }
}