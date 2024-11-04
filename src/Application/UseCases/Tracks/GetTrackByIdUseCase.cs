using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class GetTrackByGuidUseCase : IUseCase<TrackDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTrackByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<TrackDto>> Execute(Guid trackId)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();

            var track = await trackRepository.Find(
                new BaseSpecification<Track>(t => t.Id == trackId)
                .AddInclude(query => query
                    .Include(t => t.TrackSubjects)
                    .ThenInclude(ts => ts.Subject!)
                    .ThenInclude(s => s.SubjectKnowledges)
                    .ThenInclude(sk => sk.Knowledge!)
                    .ThenInclude(k => k.Materials)));

            if (track == null)
            {
                return Result<TrackDto>.Fail(ErrorMessage.NoTrackFoundWithGuid);
            }

            return Result<TrackDto>.Done(_mapper.Map<TrackDto>(track));
        }
        catch (Exception)
        {
            return Result<TrackDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
