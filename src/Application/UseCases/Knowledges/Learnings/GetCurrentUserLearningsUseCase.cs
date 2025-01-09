using Application.DTOs.SingleIdPivotEntities;
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

namespace Application.UseCases.Knowledges.Learnings;

public class GetCurrentUserLearningsParams
{
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public LearningLevel? LearningLevel { get; set; }
}

public class GetCurrentUserLearningsUseCase : IUseCase<List<LearningDto>, GetCurrentUserLearningsParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserLearningsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<List<LearningDto>>> Execute(GetCurrentUserLearningsParams parameters)
    {
        try
        {
            var userId = UserExtractor.GetUserId(_httpContextAccessor);
            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (user == null)
                return Result<List<LearningDto>>.Fail(ErrorMessage.UserNotFound);

            var learningRepository = _unitOfWork.Repository<Learning>();
            var learnings = await learningRepository.FindMany(
                new BaseSpecification<Learning>(
                    l => l.UserId == userId
                    && (string.IsNullOrEmpty(parameters.Search) || l.Knowledge!.Title.Contains(parameters.Search))
                    && (!parameters.LearningLevel.HasValue || l.LatestLearningHistory.LearningLevel == parameters.LearningLevel))
                .ApplyPaging(parameters.Page, parameters.PageSize)
                .AddOrderBy(l => l.NextReviewDate)
                .AddInclude(query => query
                    .Include(l => l.Knowledge!)
                    .Include(l => l.LearningHistories)
                )
            );
            var learningCount = await learningRepository.Count(
                new BaseSpecification<Learning>(l => l.UserId == userId)
            );

            if (!learnings.Any() && learningCount == 0)
                return Result<List<LearningDto>>.Fail(ErrorMessage.NoLearningsFound);


            return Result<List<LearningDto>>.Done(_mapper.Map<List<LearningDto>>(learnings), new Paging(parameters.Page, parameters.PageSize, learningCount));
        }
        catch (Exception)
        {
            return Result<List<LearningDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
