using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Games.GameOptions
{
    public class UpdateGameOptionRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid GameKnowledgeSubscriptionId { get; set; }

        [Required]
        public required string Value { get; set; }

        [Required]
        public int Group { get; set; } = 1;
        public bool? IsCorrect { get; set; }
    }
}