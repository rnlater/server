using Application.DTOs;
using Application.DTOs.PivotEntities;
using Application.DTOs.SingleIdPivotEntities;
using AutoMapper;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;

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

        CreateMap<Learning, LearningDto>();
        CreateMap<LearningHistory, LearningHistoryDto>();

        CreateMap<GameKnowledgeSubscription, GameKnowledgeSubscriptionDto>();
        CreateMap<Game, GameDto>();
        CreateMap<GameOption, GameOptionDto>();

        CreateMap<LearningList, LearningListDto>();
        CreateMap<LearningListKnowledge, LearningListKnowledgeDto>();

    }
}