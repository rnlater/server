using Application.DTOs;
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

namespace Application.UseCases.Games.GameOptions
{
    public class CreateGroupedGameOptionParams
    {
        public Guid GameKnowledgeSubscriptionId { get; set; }

        public List<GroupedGameOption> GroupedGameOptions { get; set; } = [];
    }

    public class GroupedGameOption
    {
        public GameOptionType Type { get; set; }
        public required string Value { get; set; }
        public bool? IsCorrect { get; set; }
    }

    public class CreateGroupedGameOptionsUseCase : IUseCase<List<GameOptionDto>, CreateGroupedGameOptionParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateGroupedGameOptionsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<GameOptionDto>>> Execute(CreateGroupedGameOptionParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId.HasValue ? await _unitOfWork.Repository<User>().GetById(userId.Value) : null;
                if (user == null)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.UserNotFound);

                var gameKnowledgeSubscription = await _unitOfWork.Repository<GameKnowledgeSubscription>().Find(
                    new BaseSpecification<GameKnowledgeSubscription>(gks =>
                        gks.Id == parameters.GameKnowledgeSubscriptionId)
                    .AddInclude(query => query
                        .Include(gks => gks.Knowledge!)
                        .Include(gks => gks.GameOptions!)));

                if (gameKnowledgeSubscription == null)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.GameKnowledgeSubscriptionNotFound);
                else if (!user.IsAdmin && gameKnowledgeSubscription.Knowledge!.CreatorId != user.Id)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.UserNotAuthorized);

                var gameOptionRepository = _unitOfWork.Repository<GameOption>();

                var questionOption = parameters.GroupedGameOptions.SingleOrDefault(p => p.Type == GameOptionType.Question);
                if (questionOption == null)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.RequireExactOneQuestion);

                var answerOptions = parameters.GroupedGameOptions.Where(p => p.Type == GameOptionType.Answer).ToList();
                if (answerOptions.Count < 2)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.RequireAtLeastTwoAnswers);

                var correctAnswers = answerOptions.Count(p => p.IsCorrect == true);
                if (correctAnswers != 1)
                    return Result<List<GameOptionDto>>.Fail(ErrorMessage.RequireExactOneCorrectAnswer);


                var nextGroup = gameKnowledgeSubscription.GameOptions.Any() ? gameKnowledgeSubscription.GameOptions.Max(go => go.Group) + 1 : 1;

                var gameOptions = new List<GameOption>();
                var nextOrder = 0;
                foreach (var param in parameters.GroupedGameOptions)
                {
                    var gameOption = new GameOption
                    {
                        GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                        Type = param.Type,
                        Value = param.Value,
                        IsCorrect = param.IsCorrect,
                        Order = param.Type == GameOptionType.Question ? null : nextOrder++,
                        Group = nextGroup
                    };
                    gameOptions.Add(gameOption);
                    await gameOptionRepository.Add(gameOption);
                }

                return Result<List<GameOptionDto>>.Done(_mapper.Map<List<GameOptionDto>>(gameOptions));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<List<GameOptionDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}