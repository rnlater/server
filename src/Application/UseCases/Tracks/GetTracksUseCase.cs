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
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
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
                .FindMany(
                    new BaseSpecification<Track>(t => parameters.Search == null || t.Name.Contains(parameters.Search))
                    .ApplyPaging(parameters.Page, parameters.PageSize));
            var trackCount = await trackRepository.Count();

            if (!tracks.Any())
            {
                return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.NoTrackFoundWithSearch);
            }

            return Result<IEnumerable<TrackDto>>.Done(tracks.Select(_mapper.Map<TrackDto>), new Paging(parameters.Page, parameters.PageSize, trackCount));
        }
        catch (Exception)
        {
            return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
