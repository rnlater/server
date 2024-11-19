using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Games;

public class CreateGameRequest
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required IFormFile Image { get; set; }
}
