using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learnings
{

    public class LearnKnowledgeRequests
    {
        [MaxLength(10)]
        public List<LearnKnowledgeRequest> GroupedKnowledges { get; set; } = [];
    }

    public class LearnKnowledgeRequest
    {
        [Required]
        public Guid KnowledgeId { get; set; }

        [Required]
        public Guid QuestionIdOne { get; set; }

        [Required]
        public required string AnswerOne { get; set; }

        [Required]
        public Guid QuestionIdTwo { get; set; }

        [Required]
        public required string AnswerTwo { get; set; }


        [Required]
        public required string Interpretation { get; set; }

        [Required]
        public required string WordMatchAnswer { get; set; }
    }
}