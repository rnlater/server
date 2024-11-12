using System;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges;

public class CreateKnowledgeRequest
{
    public required string Title { get; set; }
    [EnumDataType(typeof(KnowledgeLevel))]
    public string? Level { get; set; }
    public List<Guid> KnowledgeTypeIds { get; set; } = [];
    public List<Guid> KnowledgeTopicIds { get; set; } = [];
    public List<Guid> SubjectIds { get; set; } = [];
    public List<CreateMaterialRequest> Materials { get; set; } = [];
}

public class CreateMaterialRequest
{
    [EnumDataType(typeof(MaterialType))]
    public required string Type { get; set; }
    public required string Content { get; set; }
    public int? Order { get; set; }
    public List<CreateMaterialRequest> Children { get; set; } = [];
}
