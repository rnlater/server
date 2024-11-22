using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;
using Domain.Enums;

namespace Application.UseCases.Knowledges.Learnings;

public class LearnKnowledgeParams
{
    public Guid KnowledgeId;
    public Guid CorrectGameOptionId;
    public Guid GameOptionAnswerId;
    public required string Interpretation;
    public required string WordMatchAnswer;
}

public class LearnKnowledgeUseCase : IUseCase<Dictionary<Guid, int>, List<LearnKnowledgeParams>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LearnKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result<Dictionary<Guid, int>>> Execute(List<LearnKnowledgeParams> parameters)
    {
        try
        {
            var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
            var gameOptionRepository = _unitOfWork.Repository<GameOption>();
            var learningRepository = _unitOfWork.Repository<Learning>();
            var learningHistoryRepository = _unitOfWork.Repository<LearningHistory>();

            var knowledges = await knowledgeRepository.Count(
                new BaseSpecification<Knowledge>(k => parameters.Select(p => p.KnowledgeId).Contains(k.Id))
            );

            if (knowledges != parameters.Count)
                return Result<Dictionary<Guid, int>>.Fail(ErrorMessage.SomeKnowledgesNotFound);

            var userId = UserExtractor.GetUserId(_httpContextAccessor);
            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (userId == null)
                return Result<Dictionary<Guid, int>>.Fail(ErrorMessage.UserNotFound);

            var learningsCount = await learningRepository.Count(
                new BaseSpecification<Learning>(l => l.UserId == userId && parameters.Select(gks => gks.KnowledgeId).Contains(l.KnowledgeId))
            );
            if (learningsCount != 0)
                return Result<Dictionary<Guid, int>>.Fail(ErrorMessage.SomeKnowledgesAlreadyLearned);

            Dictionary<Guid, int> scores = [];

            foreach (var param in parameters)
            {
                var score = 0;

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
                {
                    await _unitOfWork.RollBackChangesAsync();
                    return Result<Dictionary<Guid, int>>.Fail(ErrorMessage.InvalidData);
                }

                if (param.GameOptionAnswerId == param.CorrectGameOptionId)
                    score += 50;

                if (param.WordMatchAnswer == param.Interpretation)
                    score += 50;


                var newLearning = new Learning
                {
                    UserId = userId.Value,
                    KnowledgeId = param.KnowledgeId,
                    NextReviewDate = DateTime.Now + (score != 0 ? NeededReviewTime.Level0 : NeededReviewTime.NotMemorized),
                };
                newLearning = await learningRepository.Add(newLearning);
                await learningHistoryRepository.Add(
                    new LearningHistory
                    {
                        LearningId = newLearning.Id,
                        LearningLevel = LearningLevel.LevelZero,
                        IsMemorized = score != 0,
                        PlayedGameId = correctGameOption.GameKnowledgeSubscription!.GameId,
                        Score = score,
                    });

                scores.Add(param.KnowledgeId, score);
            }

            return Result<Dictionary<Guid, int>>.Done(scores);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<Dictionary<Guid, int>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
