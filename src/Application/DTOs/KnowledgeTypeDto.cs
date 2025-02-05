using Application.DTOs.PivotEntities;

namespace Application.DTOs;

public class KnowledgeTypeDto : SingleIdEntityDto
{
    public required string Name { get; set; }
    public Guid? ParentId { get; set; }
    public KnowledgeTypeDto? Parent { get; set; }
    public ICollection<KnowledgeTypeDto> Children { get; set; } = [];
    public ICollection<KnowledgeTypeKnowledgeDto> KnowledgeTypeKnowledges { get; set; } = [];

    static public IEnumerable<KnowledgeTypeDto> MergeArrangeKnowledgeTypes(IEnumerable<KnowledgeTypeDto> KnowledgeTypes)
    {
        var KnowledgeTypeDictionary = KnowledgeTypes.ToDictionary(x => x.Id);
        var rootKnowledgeTypes = KnowledgeTypes.Where(m => m.ParentId == null).ToList();

        foreach (var KnowledgeType in KnowledgeTypes.Where(m => m.ParentId != null))
        {
            if (KnowledgeTypeDictionary.TryGetValue(KnowledgeType.ParentId!.Value, out var parentKnowledgeType))
            {
                parentKnowledgeType.Children.Add(KnowledgeType);
            }
        }

        KnowledgeTypes = rootKnowledgeTypes;

        return KnowledgeTypes;
    }
}