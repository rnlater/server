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
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Authentication, AuthenticationDto>().ReverseMap();
        CreateMap<Track, TrackDto>().ReverseMap();
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.KnowledgeCount, opt => opt.MapFrom(src => src.SubjectKnowledges.Count()))
            .ReverseMap();
        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        CreateMap<TrackSubject, TrackSubjectDto>().ReverseMap();
        CreateMap<SubjectKnowledge, SubjectKnowledgeDto>().ReverseMap();
        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
        CreateMap<KnowledgeType, KnowledgeTypeDto>().ReverseMap();
        CreateMap<KnowledgeTypeKnowledge, KnowledgeTypeKnowledgeDto>().ReverseMap();
    }
}