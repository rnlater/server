using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

public class PublicationRequestDto : SingleIdEntityDto
{
    public Guid KnowledgeId { get; set; }
    public KnowledgeDto? Knowledge { get; set; }

    [EnumDataType(typeof(PublicationRequestStatus))]
    public required string Status { get; set; }
}
