namespace Endpoint.ApiRequests.Tracks;

public class UpdateTrackRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
