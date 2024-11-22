using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Endpoint.ApiRequests.Knowledges.Learninglists
{
    public class CreateLearningListRequest
    {
        [Required]
        public required string Title { get; set; }

        [AllowNull]
        public Guid? KnowledgeId { get; set; }
    }
}