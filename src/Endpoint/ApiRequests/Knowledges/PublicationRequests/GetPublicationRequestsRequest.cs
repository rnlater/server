using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Knowledges.PublicationRequests;

public class GetPublicationRequestsRequest
{
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    [EnumDataType(typeof(PublicationRequestStatus))]
    public string? Status { get; set; }
}
