using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges.PublicationRequests;

public class UpdateKnowledgeVisibilityRequest
{
    [Required]
    public Guid KnowledgeId { get; set; }

    [EnumDataType(typeof(KnowledgeVisibility))]
    public required string Visibility { get; set; }
}
