using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.SingleIdEntities;

namespace Domain.Entities.SingleIdPivotEntities;

public class GameKnowledgeSubscription : SingleIdPivotEntity
{
    public Guid GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    public Guid KnowledgeId { get; set; }
    [ForeignKey(nameof(KnowledgeId))]
    public Knowledge? Knowledge { get; set; }

    public ICollection<GameOption> GameOptions { get; set; } = [];
}
