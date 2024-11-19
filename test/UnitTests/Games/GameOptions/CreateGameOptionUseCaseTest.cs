using Application.Mappings;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Games.GameOptions
{
    public class CreateGameOptionUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateGameOptionUseCase _createGameOptionUseCase;

        public CreateGameOptionUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);

            _createGameOptionUseCase = new CreateGameOptionUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionGroupNotFound()
        {
            var parameters = new CreateGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 1,
                IsCorrect = true
            };

            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(Enumerable.Empty<GameOption>());

            var result = await _createGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionGroupNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameOptionIsCreated()
        {
            var parameters = new CreateGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 1,
                IsCorrect = true
            };

            var gameOptions = new List<GameOption>
            {
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId, Group = 1, Order = 1, Value = "", IsCorrect = true }
            };

            var newGameOption = new GameOption
            {
                Id = Guid.NewGuid(),
                GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                Value = parameters.Value,
                Group = parameters.Group,
                IsCorrect = parameters.IsCorrect,
                Order = 2,
                Type = GameOptionType.Answer
            };

            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOptions);
            _gameOptionRepositoryMock.Setup(r => r.Add(It.IsAny<GameOption>())).ReturnsAsync(newGameOption);

            var result = await _createGameOptionUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(newGameOption.Id, result.Value.Id);
            Assert.Equal(newGameOption.Value, result.Value.Value);
        }

    }
}