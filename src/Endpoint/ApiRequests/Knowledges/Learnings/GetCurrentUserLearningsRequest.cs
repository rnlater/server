using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges.Learnings;

public class GetCurrentUserLearningsRequest
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;

    [EnumDataType(typeof(LearningLevel))]
    public string? LearningLevel { get; set; }
}
