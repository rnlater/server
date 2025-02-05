using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Subjects;

public class GetSubjectByGuidUseCase : IUseCase<SubjectDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRedisCache _cache;

    public GetSubjectByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRedisCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
    }

    public async Task<Result<SubjectDto>> Execute(Guid id)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();

            var userId = UserExtractor.GetUserId(_httpContextAccessor);
            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (user == null)
                return Result<SubjectDto>.Fail(ErrorMessage.UserNotFound);

            var subjectDto = await _cache.GetAsync<SubjectDto>($"{RedisCache.Keys.GetSubjectByGuid}_{id}");
            if (subjectDto == null)
            {
                var subject = await subjectRepository.Find(
                    new BaseSpecification<Subject>(s => s.Id == id)
                    .AddInclude(query => query
                    .Include(s => s.TrackSubjects)
                    .ThenInclude(ts => ts.Track!)
                    .Include(s => s.SubjectKnowledges)
                    .ThenInclude(sk => sk.Knowledge!)));
                if (subject == null)
                    return Result<SubjectDto>.Fail(ErrorMessage.NoSubjectFoundWithGuid);

                if (!user!.IsAdmin)
                    subject.SubjectKnowledges = subject.SubjectKnowledges
                            .Where(sk => sk.Knowledge?.Visibility == KnowledgeVisibility.Public).ToList();

                subjectDto = _mapper.Map<SubjectDto>(subject);

                await _cache.SetAsync($"{RedisCache.Keys.GetSubjectByGuid}_{id}", subjectDto);
            }

            if (!user!.IsAdmin)
            {
                foreach (var item in subjectDto.SubjectKnowledges)
                {
                    var userLearning = await _unitOfWork.Repository<Learning>().Find(
                        new BaseSpecification<Learning>(ul => ul.UserId == userId && ul.KnowledgeId == item.Knowledge!.Id).AddInclude(query => query.Include(l => l.LearningHistories))
                    );
                    item.Knowledge!.CurrentUserLearning = userLearning == null ? null : _mapper.Map<LearningDto>(userLearning);
                }
                subjectDto.UserLearningCount = subjectDto.SubjectKnowledges.Count(sk => sk.Knowledge!.CurrentUserLearning != null);
            }

            return Result<SubjectDto>.Done(subjectDto);
        }
        catch (Exception)
        {
            return Result<SubjectDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
