using Application.Mappings;
using Application.UseCases.Games;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Games
{
    public class GetGameByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetGameByGuidUseCase _getGameByGuidUseCase;

        public GetGameByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _getGameByGuidUseCase = new GetGameByGuidUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameNotFound()
        {
            // Arrange
            var gameId = Guid.NewGuid();

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync((Game?)null);

            // Act
            var result = await _getGameByGuidUseCase.Execute(gameId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoGameFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameIsFound()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var game = new Game { Id = gameId, Name = "Test Game", Description = "Test Description", ImageUrl = "test-image-url" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);

            // Act
            var result = await _getGameByGuidUseCase.Execute(gameId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameId, result.Value.Id);
            Assert.Equal(game.Name, result.Value.Name);
        }
    }
}