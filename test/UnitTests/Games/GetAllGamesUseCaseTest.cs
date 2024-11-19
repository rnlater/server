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
    public class GetAllGamesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetAllGamesUseCase _getAllGamesUseCase;

        public GetAllGamesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _getAllGamesUseCase = new GetAllGamesUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGamesAreFound()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Description = "Description 1", ImageUrl = "image-url-1" },
                new Game { Id = Guid.NewGuid(), Name = "Game 2", Description = "Description 2", ImageUrl = "image-url-2" }
            };

            _gameRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(games);

            // Act
            var result = await _getAllGamesUseCase.Execute(new NoParam());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(games.Count, result.Value.Count());
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoGamesFound()
        {
            var games = new List<Game> { };

            // Arrange
            _gameRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(games);

            // Act
            var result = await _getAllGamesUseCase.Execute(new NoParam());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoGamesFound, result.Error);
        }
    }
}