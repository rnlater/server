namespace Endpoint.ApiRequests.Knowledges.KnowledgeTypes;

public class UpdateKnowledgeTypeRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
}
