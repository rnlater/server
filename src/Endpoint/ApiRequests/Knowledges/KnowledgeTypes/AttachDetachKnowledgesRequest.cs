namespace Endpoint.ApiRequests.Knowledges.KnowledgeTypes;

public class AttachDetachKnowledgesRequest
{
    public Guid KnowledgeTypeId { get; set; }
    public List<Guid> KnowledgeIds { get; set; } = [];
}
