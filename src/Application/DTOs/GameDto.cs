using Application.DTOs.SingleIdPivotEntities;

namespace Application.DTOs
{
    public class GameDto : SingleIdEntityDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }

        public ICollection<GameKnowledgeSubscriptionDto> GameKnowledgeSubscriptions { get; set; } = [];

    }
}