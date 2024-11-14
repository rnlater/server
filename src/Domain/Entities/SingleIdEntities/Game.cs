using System;
using Domain.Entities.SingleIdPivotEntities;

namespace Domain.Entities.SingleIdEntities;

public class Game : SingleIdEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }

    public ICollection<GameKnowledgeSubscription> GameKnowledgeSubscriptions { get; set; } = [];
}
