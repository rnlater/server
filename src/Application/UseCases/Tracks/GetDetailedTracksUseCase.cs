using Application.DTOs;
using Application.DTOs.PivotEntities;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class GetDetailedTracksUseCase : IUseCase<IEnumerable<TrackDto>, NoParam>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDetailedTracksUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TrackDto>>> Execute(NoParam parameters)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();
            var subjectKnowledgeRepository = _unitOfWork.Repository<SubjectKnowledge>();

            IEnumerable<Track> tracks = await trackRepository.FindMany(
                new BaseSpecification<Track>()
                .AddInclude(query => query
                .Include(t => t.TrackSubjects)
                .ThenInclude(ts => ts.Subject!)));

            if (!tracks.Any())
            {
                return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.NoTracksFound);
            }

            var trackDtos = tracks.Select(_mapper.Map<TrackDto>).ToList();

            foreach (TrackDto? trackDto in trackDtos)
            {
                foreach (TrackSubjectDto trackSubject in trackDto.TrackSubjects)
                {
                    var knowledgeCount = await subjectKnowledgeRepository.Count(
                    new BaseSpecification<SubjectKnowledge>(sk => sk.SubjectId == trackSubject.SubjectId));

                    if (trackSubject.Subject != null)
                    {
                        trackSubject.Subject.KnowledgeCount = knowledgeCount;
                    }
                }
            }

            return Result<IEnumerable<TrackDto>>.Done(trackDtos);
        }
        catch (Exception)
        {
            return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
