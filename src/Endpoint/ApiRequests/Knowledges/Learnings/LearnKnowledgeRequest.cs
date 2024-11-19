using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learnings
{
    public class LearnKnowledgeRequest
    {
        [Required]
        public Guid KnowledgeId { get; set; }

        [Required]
        public Guid CorrectGameOptionId { get; set; }

        [Required]
        public Guid GameOptionAnswerId { get; set; }

        [Required]
        public required string Interpretation { get; set; }

        [Required]
        public required string WordMatchAnswer { get; set; }
    }
}