using Application.DTOs;
using Application.Mappings;
using Application.UseCases.Games;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Shared.Types;

namespace UnitTests.Games
{
    public class AttachGameToKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameKnowledgeSubscription>> _gameKnowledgeSubscriptionRepositoryMock;
        private readonly Mock<CreateGroupedGameOptionsUseCase> _createGroupedGameOptionsUseCaseMock;
        private readonly IMapper _mapper;
        private readonly AttachGameToKnowledgeUseCase _attachGameToKnowledgeUseCase;

        public AttachGameToKnowledgeUseCaseTest()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameKnowledgeSubscriptionRepositoryMock = new Mock<IRepository<GameKnowledgeSubscription>>();
            _createGroupedGameOptionsUseCaseMock = new Mock<CreateGroupedGameOptionsUseCase>(_unitOfWorkMock.Object, _mapper);

            _unitOfWorkMock.Setup(u => u.Repository<GameKnowledgeSubscription>()).Returns(_gameKnowledgeSubscriptionRepositoryMock.Object);

            _attachGameToKnowledgeUseCase = new AttachGameToKnowledgeUseCase(_unitOfWorkMock.Object, _mapper, _createGroupedGameOptionsUseCaseMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameKnowledgeSubscriptionAlreadyExists()
        {
            var parameters = new AttachGameToKnowledgeParams
            {
                GameId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid(),
                GroupedGameOptionsList = new List<List<GroupedGameOption>>
                {
                    new List<GroupedGameOption>
                    {
                        new GroupedGameOption { Type = GameOptionType.Question, Value = "Question 1" },
                        new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true },
                        new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 2", IsCorrect = false }
                    }
                }
            };

            var subscription = new GameKnowledgeSubscription
            {
                Id = Guid.NewGuid(),
                GameId = parameters.GameId,
                KnowledgeId = parameters.KnowledgeId
            };

            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(subscription);

            var result = await _attachGameToKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameKnowledgeSubscriptionAlreadyExists, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameIsAttachedToKnowledge()
        {
            var parameters = new AttachGameToKnowledgeParams
            {
                GameId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid(),
                GroupedGameOptionsList = new List<List<GroupedGameOption>>
                { }
            };

            var subscription = new GameKnowledgeSubscription
            {
                Id = Guid.NewGuid(),
                GameId = parameters.GameId,
                KnowledgeId = parameters.KnowledgeId
            };

            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync((GameKnowledgeSubscription?)null);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Add(It.IsAny<GameKnowledgeSubscription>())).ReturnsAsync(subscription);

            var result = await _attachGameToKnowledgeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(subscription.Id, result.Value.Id);
        }

    }
}