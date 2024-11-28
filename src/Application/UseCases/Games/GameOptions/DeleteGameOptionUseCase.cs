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
    public class DeleteGameOptionUseCase : IUseCase<GameOptionDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GameOptionDto>> Execute(Guid id)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId.HasValue ? await _unitOfWork.Repository<User>().GetById(userId.Value) : null;
                if (user == null)
                    return Result<GameOptionDto>.Fail(ErrorMessage.UserNotFound);

                var gameOptionRepository = _unitOfWork.Repository<GameOption>();
                var gameOption = await gameOptionRepository.Find(new BaseSpecification<GameOption>(go => go.Id == id));
                if (gameOption == null)
                {
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionNotFoundWithGuid);
                }

                var gameKnowledgeSubscription = await _unitOfWork.Repository<GameKnowledgeSubscription>().Find(
                    new BaseSpecification<GameKnowledgeSubscription>(gks =>
                        gks.Id == gameOption.GameKnowledgeSubscriptionId)
                    .AddInclude(query => query
                        .Include(gks => gks.Knowledge!)
                        .Include(gks => gks.GameOptions!)));

                if (gameKnowledgeSubscription == null)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameKnowledgeSubscriptionNotFound);
                else if (!user.IsAdmin && gameKnowledgeSubscription.Knowledge!.CreatorId != user.Id)
                    return Result<GameOptionDto>.Fail(ErrorMessage.UserNotAuthorized);

                var gameOptions = gameKnowledgeSubscription!.GameOptions.Where(go => go.Group == gameOption.Group);
                if (gameOptions.Count() == 0)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionGroupNotFound);

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