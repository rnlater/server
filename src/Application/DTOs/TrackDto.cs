using Application.DTOs.PivotEntities;

namespace Application.DTOs;

public class TrackDto : SingleIdEntityDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public IEnumerable<TrackSubjectDto> Subjects { get; set; } = [];
}
