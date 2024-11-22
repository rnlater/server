using System;
using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.PublicationRequests;

public class ApproveRejectPublicationRequestRequest
{
    [Required]
    public Guid PublicationRequestId { get; set; }
    [Required]
    public bool IsApproved { get; set; }
}
