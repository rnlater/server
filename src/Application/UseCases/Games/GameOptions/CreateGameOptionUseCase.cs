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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateGameOptionUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GameOptionDto>> Execute(CreateGameOptionParams parameters)
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