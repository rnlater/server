using Application.DTOs.PivotEntities;

namespace Application.DTOs
{
    public class KnowledgeTopicDto : SingleIdEntityDto
    {
        public required string Title { get; set; }
        public int? Order { get; set; }
        public Guid? ParentId { get; set; }
        public KnowledgeTopicDto? Parent { get; set; }
        public ICollection<KnowledgeTopicDto> Children { get; set; } = [];
        public ICollection<KnowledgeTopicKnowledgeDto> KnowledgeTopicKnowledges { get; set; } = [];

        public static IEnumerable<KnowledgeTopicDto> MergeArrangeKnowledgeTopics(IEnumerable<KnowledgeTopicDto> KnowledgeTopics)
        {
            var KnowledgeTopicDictionary = KnowledgeTopics.ToDictionary(x => x.Id);
            var rootKnowledgeTopics = KnowledgeTopics.Where(m => m.ParentId == null).ToList();

            foreach (var KnowledgeTopic in KnowledgeTopics.Where(m => m.ParentId != null))
            {
                if (KnowledgeTopicDictionary.TryGetValue(KnowledgeTopic.ParentId!.Value, out var parentKnowledgeTopic))
                {
                    parentKnowledgeTopic.Children.Add(KnowledgeTopic);
                }
            }

            SortKnowledgeTopics(rootKnowledgeTopics);

            KnowledgeTopics = rootKnowledgeTopics;

            return KnowledgeTopics;
        }

        private static void SortKnowledgeTopics(List<KnowledgeTopicDto> KnowledgeTopics)
        {
            if (KnowledgeTopics.Count == 0) return;

            KnowledgeTopics.Sort((x, y) => x.Order.GetValueOrDefault().CompareTo(y.Order.GetValueOrDefault()));
            foreach (var KnowledgeTopic in KnowledgeTopics)
            {
                SortKnowledgeTopics([.. KnowledgeTopic.Children]);
            }
        }
    }
}
