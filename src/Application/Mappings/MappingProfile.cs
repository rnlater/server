using Application.DTOs;
using Application.DTOs.PivotEntities;
using Application.DTOs.SingleIdPivotEntities;
using AutoMapper;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<Authentication, AuthenticationDto>();

        CreateMap<Track, TrackDto>();
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.KnowledgeCount, opt => opt.MapFrom(src => src.SubjectKnowledges.Count()));
        CreateMap<TrackSubject, TrackSubjectDto>();

        CreateMap<Knowledge, KnowledgeDto>()
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility.ToString()))
            .ForMember(dest => dest.DistinctInterpretation, opt => opt.MapFrom(src => src
                .Materials
                .Where(m => m.Type == MaterialType.Interpretation)
                .Select(m => m.Content)
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault()))
            .AfterMap((src, dest) => dest.MergeArrangeMaterials());
        CreateMap<SubjectKnowledge, SubjectKnowledgeDto>();

        CreateMap<PublicationRequest, PublicationRequestDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<KnowledgeType, KnowledgeTypeDto>();
        CreateMap<KnowledgeTypeKnowledge, KnowledgeTypeKnowledgeDto>();

        CreateMap<KnowledgeTopic, KnowledgeTopicDto>();
        CreateMap<KnowledgeTopicKnowledge, KnowledgeTopicKnowledgeDto>();

        CreateMap<Material, MaterialDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<Learning, LearningDto>()
            .ForMember(dest => dest.LatestLearningHistory, opt => opt.MapFrom(src => src.LearningHistories.OrderByDescending(lh => lh.CreatedAt).FirstOrDefault() ?? new LearningHistory(false, LearningLevel.LevelZero)));
        CreateMap<LearningHistory, LearningHistoryDto>();

        CreateMap<GameKnowledgeSubscription, GameKnowledgeSubscriptionDto>();
        CreateMap<Game, GameDto>();
        CreateMap<GameOption, GameOptionDto>();

        CreateMap<LearningList, LearningListDto>();
        CreateMap<LearningListKnowledge, LearningListKnowledgeDto>();

    }
}