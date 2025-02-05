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
    public class UpdateGameOptionParams
    {
        public Guid Id { get; set; }
        public Guid GameKnowledgeSubscriptionId { get; set; }
        public required string Value { get; set; }
        public int Group { get; set; }
        public bool? IsCorrect { get; set; }
    }

    public class UpdateGameOptionUseCase : IUseCase<GameOptionDto, UpdateGameOptionParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GameOptionDto>> Execute(UpdateGameOptionParams parameters)
        {
            try
            {
                var gameOptionRepository = _unitOfWork.Repository<GameOption>();
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId.HasValue ? await _unitOfWork.Repository<User>().GetById(userId.Value) : null;
                if (user == null)
                    return Result<GameOptionDto>.Fail(ErrorMessage.UserNotFound);

                var gameKnowledgeSubscription = await _unitOfWork.Repository<GameKnowledgeSubscription>().Find(
                    new BaseSpecification<GameKnowledgeSubscription>(gks =>
                        gks.Id == parameters.GameKnowledgeSubscriptionId)
                    .AddInclude(query => query
                        .Include(gks => gks.Knowledge!)
                        .Include(gks => gks.GameOptions!)));

                if (gameKnowledgeSubscription == null)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameKnowledgeSubscriptionNotFound);
                else if (!user.IsAdmin && gameKnowledgeSubscription.Knowledge!.CreatorId != user.Id)
                    return Result<GameOptionDto>.Fail(ErrorMessage.UserNotAuthorized);

                var gameOptions = gameKnowledgeSubscription!.GameOptions.Where(go => go.Group == parameters.Group);
                if (gameOptions.Count() == 0)
                    return Result<GameOptionDto>.Fail(ErrorMessage.GameOptionGroupNotFound);

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