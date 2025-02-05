using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.PublicationRequests;

public class RequestPublishKnowledgeRequest
{
    [Required]
    public Guid KnowledgeId { get; set; }

}
