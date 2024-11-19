using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;

namespace Domain.Entities.SingleIdEntities;

public class LearningHistory : SingleIdEntity
{
    public LearningHistory() : base() { }
    public LearningHistory(bool IsMemorized, LearningLevel learningLevel) : base()
    {
        this.IsMemorized = IsMemorized;
        this.LearningLevel = learningLevel;
    }

    public Guid LearningId { get; set; }
    [ForeignKey(nameof(LearningId))]
    public Learning? Learning { get; set; }

    public LearningLevel LearningLevel { get; set; }
    public bool IsMemorized { get; set; }

    public Guid PlayedGameId { get; set; }
    [ForeignKey(nameof(PlayedGameId))]
    public Game? PlayedGame { get; set; }

    public int Score { get; set; }
}
