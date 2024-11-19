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
    public class UpdateGameOptionParams
    {
        public Guid Id { get; set; }
        public Guid GameKnowledgeSubscriptionId { get; set; }
        public required string Value { get; set; }
        public int Group { get; set; } = 1;
        public bool? IsCorrect { get; set; }
    }

    public class UpdateGameOptionUseCase : IUseCase<GameOptionDto, UpdateGameOptionParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GameOptionDto>> Execute(UpdateGameOptionParams parameters)
        {
            try
            {
                var gameOptionRepository = _unitOfWork.Repository<GameOption>();

                var gameOptions = await gameOptionRepository.FindMany(new BaseSpecification<GameOption>(go =>
               go.Group == parameters.Group && go.GameKnowledgeSubscriptionId == parameters.GameKnowledgeSubscriptionId));

                var gameOption = gameOptions.FirstOrDefault(go => go.Id == parameters.Id);
                if (gameOption == null)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionNotFoundWithGuid);

                gameOption.Value = parameters.Value;
                if (gameOption.Type != GameOptionType.Question && gameOption.IsCorrect == false && parameters.IsCorrect == true)
                {
                    var correctOption = gameOptions.FirstOrDefault(go => go.IsCorrect == true);
                    correctOption!.IsCorrect = false;
                    gameOption.IsCorrect = true;

                    await gameOptionRepository.Update(correctOption);
                }

                gameOption = await gameOptionRepository.Update(gameOption);

                return Result<GameOptionDto>.Done(_mapper.Map<GameOptionDto>(gameOption));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<GameOptionDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}