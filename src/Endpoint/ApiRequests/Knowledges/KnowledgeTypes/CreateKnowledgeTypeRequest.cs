namespace Endpoint.ApiRequests.Knowledges.KnowledgeTypes;

public class CreateKnowledgeTypeRequest
{
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public List<Guid> KnowledgeIds { get; set; } = [];
}
