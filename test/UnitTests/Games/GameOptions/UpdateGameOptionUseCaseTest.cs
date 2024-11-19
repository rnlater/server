using Application.Mappings;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Games.GameOptions
{
    public class UpdateGameOptionUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateGameOptionUseCase _updateGameOptionUseCase;

        public UpdateGameOptionUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);

            _updateGameOptionUseCase = new UpdateGameOptionUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionNotFound()
        {
            var parameters = new UpdateGameOptionParams
            {
                Id = Guid.NewGuid(),
                Value = "Updated Answer",
                Group = 1,
                IsCorrect = true
            };

            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync([]);

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionNotFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameOptionIsUpdated()
        {
            var gameOptionId = Guid.NewGuid();
            var parameters = new UpdateGameOptionParams
            {
                Id = gameOptionId,
                Value = "Updated Answer",
                Group = 1,
                IsCorrect = true
            };

            var gameOption = new GameOption
            {
                Id = gameOptionId,
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Old Answer",
                Group = 1,
                IsCorrect = false
            };

            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(new List<GameOption> { gameOption });
            _gameOptionRepositoryMock.Setup(r => r.Update(It.IsAny<GameOption>())).ReturnsAsync(gameOption);

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameOptionId, result.Value.Id);
            Assert.Equal(parameters.Value, result.Value.Value);
        }
    }
}