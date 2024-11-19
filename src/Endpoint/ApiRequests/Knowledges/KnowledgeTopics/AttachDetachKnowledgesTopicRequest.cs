namespace Endpoint.ApiRequests.Knowledges.KnowledgeTopics;

public class AttachDetachKnowledgesTopicRequest
{
    public Guid KnowledgeTopicId { get; set; }
    public List<Guid> KnowledgeIds { get; set; } = [];
}
