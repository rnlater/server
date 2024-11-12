using Application.DTOs;
using Application.DTOs.PivotEntities;
using AutoMapper;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;

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
        CreateMap<Knowledge, KnowledgeDto>()
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility.ToString()));
        CreateMap<Knowledge, KnowledgeDto>()
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
            .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => src.Visibility.ToString()))
            .AfterMap((src, dest) => dest.MergeArrangeMaterials());
        CreateMap<TrackSubject, TrackSubjectDto>();
        CreateMap<SubjectKnowledge, SubjectKnowledgeDto>();
        CreateMap<KnowledgeType, KnowledgeTypeDto>();
        CreateMap<KnowledgeTypeKnowledge, KnowledgeTypeKnowledgeDto>();
        CreateMap<KnowledgeTopic, KnowledgeTopicDto>();
        CreateMap<KnowledgeTopicKnowledge, KnowledgeTopicKnowledgeDto>();
        CreateMap<Material, MaterialDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
    }
}