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

public class ReviewLearningParams
{
    public Guid KnowledgeId;
    public Guid CorrectGameOptionId;
    public Guid GameOptionAnswerId;
    public required string Interpretation;
    public required string WordMatchAnswer;
}

public class ReviewLearningUseCase : IUseCase<List<LearningDto>, List<ReviewLearningParams>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReviewLearningUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<List<LearningDto>>> Execute(List<ReviewLearningParams> parameters)
    {
        try
        {
            var gameOptionRepository = _unitOfWork.Repository<GameOption>();
            var learningRepository = _unitOfWork.Repository<Learning>();
            var learningHistoryRepository = _unitOfWork.Repository<LearningHistory>();

            Guid? userId = UserExtractor.GetUserId(_httpContextAccessor);
            if (userId == null)
                return Result<List<LearningDto>>.Fail(ErrorMessage.UserNotFound);

            List<LearningDto> learningsResponse = [];

            foreach (var param in parameters)
            {
                var score = 0;

                var learning = await learningRepository.Find(
                    new BaseSpecification<Learning>(l => l.UserId == userId && param.KnowledgeId == l.KnowledgeId)
                    .AddInclude(query => query.Include(l => l.LearningHistories)));
                if (learning == null)
                    return Result<List<LearningDto>>.Fail(ErrorMessage.LearningNotFound);
                else if (learning.NextReviewDate > DateTime.Now)
                    return Result<List<LearningDto>>.Fail(ErrorMessage.KnowledgeNotReadyToReview);
                else if (learning.LearningHistories.Count == 0)
                    return Result<List<LearningDto>>.Fail(ErrorMessage.RequireLearningBeforeReview);

                var gameOptions = await gameOptionRepository.FindMany(
                    new BaseSpecification<GameOption>(go =>
                        go.Id == param.GameOptionAnswerId
                        || go.Id == param.CorrectGameOptionId)
                    .AddInclude(query => query
                        .Include(go => go.GameKnowledgeSubscription!)
                        .ThenInclude(gks => gks.Knowledge!)
                        .ThenInclude(k => k.Materials))
                );
                var correctGameOption = gameOptions.First(go => go.Id == param.CorrectGameOptionId);

                if (gameOptions == null
                || gameOptions.Count() != 2
                || correctGameOption.GameKnowledgeSubscription!.Knowledge!.Id != param.KnowledgeId
                || !correctGameOption.GameKnowledgeSubscription!.Knowledge!.Materials.Select(m => m.Content).Contains(param.Interpretation))
                    return Result<List<LearningDto>>.Fail(ErrorMessage.InvalidData);

                if (param.GameOptionAnswerId == param.CorrectGameOptionId)
                    score += 50;

                if (param.WordMatchAnswer == param.Interpretation)
                    score += 50;

                var LatestLearningHistory = learning.LatestLearningHistory;
                var IsMemorized = score != 0;

                learning.NextReviewDate = DateTime.Now + (IsMemorized ? GetNextReviewTime(LatestLearningHistory!.LearningLevel) : NeededReviewTime.NotMemorized);
                await learningRepository.Update(learning);

                await learningHistoryRepository.Add(
                    new LearningHistory
                    {
                        LearningId = learning.Id,
                        LearningLevel = IsMemorized ? GetNextLevel(LatestLearningHistory!.LearningLevel) : LatestLearningHistory!.LearningLevel,
                        IsMemorized = IsMemorized,
                        PlayedGameId = correctGameOption.GameKnowledgeSubscription!.GameId,
                        Score = score,
                    });

                learningsResponse.Add(_mapper.Map<LearningDto>(learning));
            }

            return Result<List<LearningDto>>.Done(learningsResponse);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<List<LearningDto>>.Fail(ErrorMessage.UnknownError);
        }
    }

    public static TimeSpan GetNextReviewTime(LearningLevel level)
    {
        return level switch
        {
            LearningLevel.LevelZero => NeededReviewTime.Level0,
            LearningLevel.LevelOne => NeededReviewTime.Level1,
            LearningLevel.LevelTwo => NeededReviewTime.Level2,
            LearningLevel.LevelThree => NeededReviewTime.Level3,
            LearningLevel.LevelFour => NeededReviewTime.Level4,
            LearningLevel.LevelFive => NeededReviewTime.Level5,
            _ => NeededReviewTime.Level0
        };
    }
    public static LearningLevel GetNextLevel(LearningLevel level)
    {
        return level switch
        {
            LearningLevel.LevelZero => LearningLevel.LevelOne,
            LearningLevel.LevelOne => LearningLevel.LevelTwo,
            LearningLevel.LevelTwo => LearningLevel.LevelThree,
            LearningLevel.LevelThree => LearningLevel.LevelFour,
            LearningLevel.LevelFour => LearningLevel.LevelFive,
            LearningLevel.LevelFive => LearningLevel.LevelFive,
            _ => LearningLevel.LevelZero
        };
    }
}