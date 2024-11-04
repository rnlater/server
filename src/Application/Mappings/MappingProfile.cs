using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Authentication, AuthenticationDto>().ReverseMap();
        CreateMap<Track, TrackDto>().ReverseMap();
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Knowledge, KnowledgeDto>().ReverseMap();
    }
}