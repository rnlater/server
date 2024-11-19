using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Games.GameOptions
{
    public class CreateGameOptionRequest
    {
        [Required]
        public Guid GameKnowledgeSubscriptionId { get; set; }

        [Required]
        public required string Value { get; set; }

        [Required]
        public int Group { get; set; }
        public bool? IsCorrect { get; set; }
    }
}