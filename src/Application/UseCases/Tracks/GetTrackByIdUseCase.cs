using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Tracks;

public class GetTrackByGuidUseCase : IUseCase<TrackDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRedisCache _cache;

    public GetTrackByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRedisCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
    }

    public async Task<Result<TrackDto>> Execute(Guid trackId)
    {
        try
        {
            var userId = UserExtractor.GetUserId(_httpContextAccessor);

            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (user == null)
                return Result<TrackDto>.Fail(ErrorMessage.UserNotFound);

            var trackRepository = _unitOfWork.Repository<Track>();
            var subjectKnowledgeRepository = _unitOfWork.Repository<SubjectKnowledge>();
            var learningRepository = _unitOfWork.Repository<Learning>();

            var trackDto = await _cache.GetAsync<TrackDto>($"{RedisCache.Keys.GetTrackByGuid}_{trackId}");
            if (trackDto == null)
            {
                var track = await trackRepository.Find(
                    new BaseSpecification<Track>(t => t.Id == trackId)
                    .AddInclude(query => query
                    .Include(t => t.TrackSubjects)
                    .ThenInclude(ts => ts.Subject!)
                    .ThenInclude(s => s.SubjectKnowledges)
                    .ThenInclude(sk => sk.Knowledge!)));
                if (track == null)
                {
                    return Result<TrackDto>.Fail(ErrorMessage.NoTrackFoundWithGuid);
                }

                if (!user!.IsAdmin)
                {
                    foreach (var item in track.TrackSubjects)
                    {
                        item.Subject!.SubjectKnowledges = item.Subject.SubjectKnowledges
                            .Where(sk => sk.Knowledge?.Visibility == KnowledgeVisibility.Public).ToList();
                        foreach (var sk in item.Subject.SubjectKnowledges)
                        {
                            sk.Knowledge = null;
                        }
                    }
                }

                trackDto = _mapper.Map<TrackDto>(track);

                if (!user!.IsAdmin)
                {
                    foreach (var item in trackDto.TrackSubjects)
                    {
                        item.Subject!.UserLearningCount = await learningRepository.Count(
                            new BaseSpecification<Learning>(l => l.UserId == userId && item.Subject.SubjectKnowledges.Select(sk => sk.KnowledgeId).Contains(l.KnowledgeId)));
                    }
                }

                await _cache.SetAsync($"{RedisCache.Keys.GetTrackByGuid}_{trackId}", trackDto);
            }

            return Result<TrackDto>.Done(trackDto);
        }
        catch (Exception)
        {
            return Result<TrackDto>.Fail(ErrorMessage.UnknownError);
        }
    }

}
