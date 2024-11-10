using Application.DTOs.PivotEntities;

namespace Application.DTOs
{
    public class KnowledgeTopicDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int? Order { get; set; }
        public Guid? ParentId { get; set; }
        public KnowledgeTopicDto? Parent { get; set; }
        public ICollection<KnowledgeTopicDto> Children { get; set; } = [];
        public ICollection<KnowledgeTopicKnowledgeDto> KnowledgeTopicKnowledges { get; set; } = [];
    }
}
