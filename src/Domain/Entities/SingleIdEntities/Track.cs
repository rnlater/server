namespace Domain.Entities.SingleIdEntities;

using System.Collections.Generic;
using Domain.Entities.PivotEntities;

public class Track : SingleIdEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<TrackSubject> TrackSubjects { get; set; } = [];

    public static Track ForTestPurposeOnly()
    {
        return new Track
        {
            Id = Guid.NewGuid(),
            Name = "Test Track",
            Description = "Test Description",
        };
    }
}
