using System;

namespace Endpoint.ApiRequests.Knowledges;

public class AttachDeattachKnowledgeTypeRequest
{
    public Guid KnowledgeId { get; set; }
    public Guid KnowledgeTypeId { get; set; }
}
