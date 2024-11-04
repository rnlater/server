using Domain.Entities.PivotEntities;

namespace Domain.Entities.SingleIdEntities;

public class Subject : SingleIdEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Photo { get; set; }

    public ICollection<TrackSubject> TrackSubjects { get; set; } = [];
    public ICollection<SubjectKnowledge> SubjectKnowledges { get; set; } = [];

    public static Subject ForTestPurposeOnly()
    {
        return new Subject
        {
            Id = Guid.NewGuid(),
            Name = "Test Subject",
            Photo = "Test Photo",
            Description = "Test Description",
        };
    }
}
