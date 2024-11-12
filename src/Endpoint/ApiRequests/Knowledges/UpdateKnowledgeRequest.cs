using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges;

public class UpdateKnowledgeRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    [EnumDataType(typeof(KnowledgeLevel))]
    public string? Level { get; set; }
}
