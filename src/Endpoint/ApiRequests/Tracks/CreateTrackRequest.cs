namespace Endpoint.ApiRequests.Tracks;

public class CreateTrackRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Guid> SubjectGuids { get; set; } = [];
}
