using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Endpoint.ApiRequests.Games.GameOptions
{
    public class CreateGroupedGameOptionRequest
    {
        [Required]
        public Guid GameKnowledgeSubscriptionId { get; set; }

        [MinLength(1)]
        public List<GroupedGameOptionRequest> GroupedGameOptions { get; set; } = [];
    }

    public class GroupedGameOptionRequest
    {
        [Required]
        [EnumDataType(typeof(GameOptionType))]
        public required string Type { get; set; }

        [Required]
        public required string Value { get; set; }

        [Required]
        public bool? IsCorrect { get; set; }
    }

}