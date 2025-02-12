using System.ComponentModel.DataAnnotations;
using Application.UseCases.Knowledges;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges
{
    public class SearchKnowledgesRequest
    {
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<Guid>? KnowledgeTypeIds { get; set; }
        public List<Guid>? KnowledgeTopicIds { get; set; }

        [EnumDataType(typeof(KnowledgeLevel))]
        public string? Level { get; set; }

        [EnumDataType(typeof(SearchKnowledgesParams.OrderByType))]
        public string OrderBy { get; set; } = "Date";

        public bool? Ascending { get; set; }
    }
}
