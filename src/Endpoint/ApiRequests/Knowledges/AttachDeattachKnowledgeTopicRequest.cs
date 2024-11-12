using System;

namespace Endpoint.ApiRequests.Knowledges;

public class AttachDeattachKnowledgeTopicRequest
{
    public Guid KnowledgeId { get; set; }
    public Guid KnowledgeTopicId { get; set; }
}
