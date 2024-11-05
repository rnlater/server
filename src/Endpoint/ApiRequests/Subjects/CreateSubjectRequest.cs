using System;

namespace Endpoint.ApiRequests.Subjects;

public class CreateSubjectRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required IFormFile Photo { get; set; }
    public List<Guid> TrackUids { get; set; } = [];
    public List<Guid> KnowledgeUids { get; set; } = [];
}
