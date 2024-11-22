using System.ComponentModel.DataAnnotations;
using Application.DTOs.PivotEntities;
using Application.DTOs.SingleIdPivotEntities;
using Domain.Enums;

namespace Application.DTOs;

public class KnowledgeDto : SingleIdEntityDto
{
    public required string Title { get; set; }
    [EnumDataType(typeof(KnowledgeVisibility))]
    public required string Visibility { get; set; }
    [EnumDataType(typeof(KnowledgeLevel))]
    public required string Level { get; set; }
    public Guid CreatorId { get; set; }
    public UserDto? Creator { get; set; }
    public PublicationRequestDto? PublicationRequest { get; set; }
    public ICollection<MaterialDto> Materials { get; set; } = [];
    public ICollection<SubjectKnowledgeDto> SubjectKnowledges { get; set; } = [];
    public ICollection<KnowledgeTypeKnowledgeDto> KnowledgeTypeKnowledges { get; set; } = [];
    public ICollection<KnowledgeTopicKnowledgeDto> KnowledgeTopicKnowledges { get; set; } = [];
    public ICollection<LearningDto> Learnings { get; set; } = [];
    public ICollection<GameKnowledgeSubscriptionDto> GameKnowledgeSubscriptions { get; set; } = [];
    public GameKnowledgeSubscriptionDto? GameToReview { get; set; }
    public ICollection<LearningListKnowledgeDto> LearningListKnowledges { get; set; } = [];

    public void MergeArrangeMaterials()
    {
        var materialDictionary = Materials.ToDictionary(x => x.Id);
        var rootMaterials = Materials.Where(m => m.ParentId == null).ToList();

        foreach (var material in Materials.Where(m => m.ParentId != null))
        {
            if (materialDictionary.TryGetValue(material.ParentId!.Value, out var parentMaterial))
            {
                parentMaterial.Children.Add(material);
            }
        }

        SortMaterials(rootMaterials);

        Materials = rootMaterials;
    }

    private static void SortMaterials(List<MaterialDto> materials)
    {
        if (materials.Count == 0) return;

        materials.Sort((x, y) => x.Order.GetValueOrDefault().CompareTo(y.Order.GetValueOrDefault()));
        foreach (var material in materials)
        {
            SortMaterials([.. material.Children]);
        }
    }
}
