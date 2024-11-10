namespace Endpoint.ApiRequests.Subjects;

public class UpdateSubjectRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public IFormFile? Photo { get; set; }
}
