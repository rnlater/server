namespace Endpoint.ApiRequests.Tracks;

public class CreateDeleteTrackSubjectRequest
{
    public Guid TrackId { get; set; }
    public Guid SubjectId { get; set; }
}
