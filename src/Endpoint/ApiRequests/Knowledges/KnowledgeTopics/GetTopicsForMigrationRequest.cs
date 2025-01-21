using System.Diagnostics.CodeAnalysis;

namespace Endpoint.ApiRequests.Knowledges.KnowledgeTopics
{
    public class GetTopicsForMigrationRequest
    {
        [AllowNull]
        public Guid? ParentId { get; set; }

    }
}