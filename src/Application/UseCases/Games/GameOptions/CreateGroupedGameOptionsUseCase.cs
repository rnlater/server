using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

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

        public CreateGroupedGameOptionsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GameOptionDto>>> Execute(CreateGroupedGameOptionParams parameters)
        {
            try
            {
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

                var existingOptions = await gameOptionRepository.FindMany(new BaseSpecification<GameOption>(
                    go => go.GameKnowledgeSubscriptionId == parameters.GameKnowledgeSubscriptionId
                ));
                var nextGroup = existingOptions.Any() ? existingOptions.Max(go => go.Group) + 1 : 1;

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