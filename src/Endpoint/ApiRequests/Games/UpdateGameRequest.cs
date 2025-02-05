using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Games;

public class UpdateGameRequest
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    public IFormFile? Image { get; set; }
}
