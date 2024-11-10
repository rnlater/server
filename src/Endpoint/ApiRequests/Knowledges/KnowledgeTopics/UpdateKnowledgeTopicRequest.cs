namespace Endpoint.ApiRequests.Knowledges.KnowledgeTopics;

public class UpdateKnowledgeTopicRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public int? Order { get; set; }
    public Guid? ParentId { get; set; }
}
