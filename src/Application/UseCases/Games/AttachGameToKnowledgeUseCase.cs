using Application.DTOs.SingleIdPivotEntities;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Games
{
    public class AttachGameToKnowledgeParams
    {
        public Guid GameId { get; set; }
        public Guid KnowledgeId { get; set; }
        public List<List<GroupedGameOption>> GroupedGameOptionsList { get; set; } = [];
    }

    public class AttachGameToKnowledgeUseCase : IUseCase<GameKnowledgeSubscriptionDto, AttachGameToKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateGroupedGameOptionsUseCase _createGroupedGameOptionsUseCase;

        public AttachGameToKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper, CreateGroupedGameOptionsUseCase createGroupedGameOptionsUseCase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createGroupedGameOptionsUseCase = createGroupedGameOptionsUseCase;
        }
        public async Task<Result<GameKnowledgeSubscriptionDto>> Execute(AttachGameToKnowledgeParams parameters)
        {
            try
            {
                var gameKnowledgeSubscriptionRepository = _unitOfWork.Repository<GameKnowledgeSubscription>();

                var existingSubscription = await gameKnowledgeSubscriptionRepository.Find(new BaseSpecification<GameKnowledgeSubscription>
                (x => x.GameId == parameters.GameId && x.KnowledgeId == parameters.KnowledgeId));
                if (existingSubscription != null)
                {
                    return Result<GameKnowledgeSubscriptionDto>.Fail(ErrorMessage.GameKnowledgeSubscriptionAlreadyExists);
                }

                var subscription = new GameKnowledgeSubscription
                {
                    GameId = parameters.GameId,
                    KnowledgeId = parameters.KnowledgeId
                };
                subscription = await gameKnowledgeSubscriptionRepository.Add(subscription);

                foreach (List<GroupedGameOption> groupedGameOptions in parameters.GroupedGameOptionsList)
                {
                    var createGroupedGameOptionParams = new CreateGroupedGameOptionParams
                    {
                        GameKnowledgeSubscriptionId = subscription.Id,
                        GroupedGameOptions = groupedGameOptions
                    };
                    var gameOptions = await _createGroupedGameOptionsUseCase.Execute(createGroupedGameOptionParams);
                    if (!gameOptions.IsSuccess)
                    {
                        await _unitOfWork.RollBackChangesAsync();

                        return Result<GameKnowledgeSubscriptionDto>.Fail(gameOptions.Error);
                    }
                }

                return Result<GameKnowledgeSubscriptionDto>.Done(_mapper.Map<GameKnowledgeSubscriptionDto>(subscription));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<GameKnowledgeSubscriptionDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}