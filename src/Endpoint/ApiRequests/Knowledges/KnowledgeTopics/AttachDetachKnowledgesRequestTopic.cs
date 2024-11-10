namespace Endpoint.ApiRequests.Knowledges.KnowledgeTopics;

public class AttachDetachKnowledgesRequestTopic
{
    public Guid KnowledgeTopicId { get; set; }
    public List<Guid> KnowledgeIds { get; set; } = [];
}
