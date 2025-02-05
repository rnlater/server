using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Endpoint.ApiRequests.Knowledges
{
    public class GetKnowledgesToLearnRequest
    {
        [MaxLength(200)]
        [MinLength(1)]
        public List<Guid> KnowledgeIds { get; set; } = [];

        [AllowNull]
        public string? NewLearningListTitle { get; set; }

    }
}