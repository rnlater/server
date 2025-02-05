using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class GetDetailedTracksUseCase : IUseCase<IEnumerable<TrackDto>, NoParam>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisCache _cache;

    public GetDetailedTracksUseCase(IUnitOfWork unitOfWork, IMapper mapper, IRedisCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<TrackDto>>> Execute(NoParam parameters)
    {
        try
        {
            var trackSubjectRepository = _unitOfWork.Repository<TrackSubject>();

            var trackDtos = await _cache.GetAsync<IEnumerable<TrackDto>>(RedisCache.Keys.GetDetailedTracks);
            if (trackDtos == null)
            {
                var trackRepository = _unitOfWork.Repository<Track>();
                var tracks = await trackRepository.GetAll();

                if (!tracks.Any())
                {
                    return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.NoTracksFound);
                }

                trackDtos = tracks.Select(_mapper.Map<TrackDto>).ToList();

                foreach (var trackDto in trackDtos)
                {
                    trackDto.SubjectCount = await trackSubjectRepository.Count(
                    new BaseSpecification<TrackSubject>(ts => ts.TrackId == trackDto.Id));
                }

                await _cache.SetAsync(RedisCache.Keys.GetDetailedTracks, trackDtos, RedisCache.MostUsedCacheExpiry);
            }

            return Result<IEnumerable<TrackDto>>.Done(trackDtos);
        }
        catch (Exception)
        {
            return Result<IEnumerable<TrackDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
