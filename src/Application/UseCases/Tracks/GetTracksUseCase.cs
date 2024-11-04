using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class GetTracksParams
{
    public string? Search { get; set; }
}

public class GetTracksUseCase : IUseCase<IEnumerable<TrackDto>, GetTracksParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTracksUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TrackDto>>> Execute(GetTracksParams parameters)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();

            var tracks = await trackRepository
                .FindMany(new BaseSpecification<Track>(t => parameters.Search == null || t.Name.Contains(parameters.Search)));

            if (!tracks.Any())
            {
                return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.NoTrackFoundWithSearch);
            }

            return Result<IEnumerable<TrackDto>>.Done(tracks.Select(_mapper.Map<TrackDto>));
        }
        catch (Exception)
        {
            return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
