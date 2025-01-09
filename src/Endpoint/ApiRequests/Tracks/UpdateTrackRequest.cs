using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Tracks;

public class UpdateTrackRequest
{
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
}
