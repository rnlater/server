using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;

namespace Domain.Entities.SingleIdEntities;

public class GameOption : SingleIdEntity
{
    public GameOption() : base() { }

    public Guid GameKnowledgeSubscriptionId { get; set; }
    [ForeignKey(nameof(GameKnowledgeSubscriptionId))]
    public GameKnowledgeSubscription? GameKnowledgeSubscription { get; set; }

    public GameOptionType Type { get; set; }
    public required string Value { get; set; }
    public int Group { get; set; } = 1;
    public bool? IsCorrect { get; set; }
    public int? Order { get; set; }
}
