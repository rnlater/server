namespace Application.DTOs.SingleIdPivotEntities
{
    public class GameKnowledgeSubscriptionDto : SingleIdPivotEntityDto
    {
        public Guid GameId { get; set; }
        public GameDto? Game { get; set; }
        public Guid KnowledgeId { get; set; }
        public KnowledgeDto? Knowledge { get; set; }
        public ICollection<GameOptionDto> GameOptions { get; set; } = [];

        public GameOptionDto GetCorrectGameOption()
        {
            return GameOptions.First(go => go.IsCorrect == true);
        }

        public GameKnowledgeSubscriptionDto DistinctGroupedGameOptions()
        {
            var groupedOptions = GameOptions.GroupBy(go => go.Group).ToList();
            var selectedGroup = groupedOptions[new Random().Next(groupedOptions.Count)];
            GameOptions = [.. selectedGroup];
            return this;
        }

    }
}