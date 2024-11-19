using System.ComponentModel.DataAnnotations;
using Application.DTOs.SingleIdPivotEntities;
using Domain.Enums;

namespace Application.DTOs
{
    public class LearningHistoryDto : SingleIdEntityDto
    {
        public Guid LearningId { get; set; }
        public LearningDto? Learning { get; set; }
        [EnumDataType(typeof(LearningLevel))]
        public required string LearningLevel { get; set; }
        public bool IsMemorized { get; set; }
        public Guid PlayedGameId { get; set; }

        public GameDto? PlayedGame { get; set; }
        public int Score { get; set; }
    }
}