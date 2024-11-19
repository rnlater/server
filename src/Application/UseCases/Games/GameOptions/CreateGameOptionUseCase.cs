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
    public class CreateGameOptionParams
    {
        public Guid GameKnowledgeSubscriptionId { get; set; }
        public required string Value { get; set; }
        public int Group { get; set; }
        public bool? IsCorrect { get; set; }
    }

    public class CreateGameOptionUseCase : IUseCase<GameOptionDto, CreateGameOptionParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GameOptionDto>> Execute(CreateGameOptionParams parameters)
        {
            try
            {
                var gameOptionRepository = _unitOfWork.Repository<GameOption>();

                var gameOptions = await gameOptionRepository.FindMany(new BaseSpecification<GameOption>(go =>
                go.Group == parameters.Group && go.GameKnowledgeSubscriptionId == parameters.GameKnowledgeSubscriptionId));

                if (gameOptions.Count() == 0)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionGroupNotFound);
                else if (parameters.IsCorrect == true)
                {
                    var correctOption = gameOptions.FirstOrDefault(go => go.IsCorrect == true);
                    correctOption!.IsCorrect = false;
                    await gameOptionRepository.Update(correctOption);
                }

                var nextOrder = gameOptions.Max(go => go.Order) + 1;
                var newGameOption = await gameOptionRepository.Add(new GameOption
                {
                    GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                    Value = parameters.Value,
                    Group = parameters.Group,
                    IsCorrect = parameters.IsCorrect,
                    Order = nextOrder,
                    Type = GameOptionType.Answer
                });

                return Result<GameOptionDto>.Done(_mapper.Map<GameOptionDto>(newGameOption));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<GameOptionDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}