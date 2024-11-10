namespace Endpoint.ApiRequests.Knowledges.KnowledgeTopics;

public class CreateKnowledgeTopicRequest
{
    public required string Title { get; set; }
    public int? Order { get; set; }
    public Guid? ParentId { get; set; }
    public List<Guid> KnowledgeIds { get; set; } = [];
}
