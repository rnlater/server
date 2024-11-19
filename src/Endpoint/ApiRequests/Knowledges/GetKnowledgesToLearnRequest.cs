using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges
{
    public class GetKnowledgesToLearnRequest
    {
        [MaxLength(200)]
        [MinLength(1)]
        public List<Guid> KnowledgeIds { get; set; } = [];
    }
}