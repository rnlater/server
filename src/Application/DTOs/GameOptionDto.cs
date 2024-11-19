using System.ComponentModel.DataAnnotations;
using Application.DTOs.SingleIdPivotEntities;
using Domain.Enums;

namespace Application.DTOs
{
    public class GameOptionDto : SingleIdEntityDto
    {
        public Guid GameKnowledgeSubscriptionId { get; set; }
        public GameKnowledgeSubscriptionDto? GameKnowledgeSubscription { get; set; }

        [EnumDataType(typeof(GameOptionType))]
        public required string Type { get; set; }
        public required string Value { get; set; }
        public int Group { get; set; }
        public bool? IsCorrect { get; set; }
        public int? Order { get; set; }
    }
}