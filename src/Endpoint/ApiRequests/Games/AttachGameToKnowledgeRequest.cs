using System.ComponentModel.DataAnnotations;
using Endpoint.ApiRequests.Games.GameOptions;

namespace Endpoint.ApiRequests.Games
{
    public class AttachGameToKnowledgeRequest
    {
        [Required]
        public Guid GameId { get; set; }

        [Required]
        public Guid KnowledgeId { get; set; }

        [MinLength(1), Required]
        public List<List<GroupedGameOptionRequest>> GroupedGameOptionsList { get; set; } = [];
    }
}