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
    public class DeleteGameOptionUseCase : IUseCase<GameOptionDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GameOptionDto>> Execute(Guid id)
        {
            try
            {
                var gameOptionRepository = _unitOfWork.Repository<GameOption>();
                var gameOption = await gameOptionRepository.Find(new BaseSpecification<GameOption>(go => go.Id == id));
                if (gameOption == null)
                {
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionNotFoundWithGuid);
                }

                var gameOptions = await gameOptionRepository.FindMany(new BaseSpecification<GameOption>(go =>
                              go.Group == gameOption.Group && go.GameKnowledgeSubscriptionId == gameOption.GameKnowledgeSubscriptionId));

                if (gameOption.Type == GameOptionType.Answer)
                {
                    if (gameOption.IsCorrect == true)
                        return Result<GameOptionDto>.Fail(ErrorMessage.CannotDeleteCorrectAnswer);
                    else if (gameOptions.Count() < 4)
                        return Result<GameOptionDto>.Fail(ErrorMessage.RequireAtLeastTwoAnswers);

                    gameOption = await gameOptionRepository.Delete(id);
                }
                else if (gameOption.Type == GameOptionType.Question)
                {
                    gameOption = await gameOptionRepository.Delete(id);
                    foreach (var option in gameOptions.Where(go => go.Type == GameOptionType.Answer))
                    {
                        await gameOptionRepository.Delete(option.Id);
                    }
                }

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