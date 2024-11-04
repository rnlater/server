namespace Application.DTOs.PivotEntities;

public class TrackSubjectDto
{
    public Guid TrackId { get; set; }
    public TrackDto? Track { get; set; }

    public Guid SubjectId { get; set; }
    public SubjectDto? Subject { get; set; }
}
