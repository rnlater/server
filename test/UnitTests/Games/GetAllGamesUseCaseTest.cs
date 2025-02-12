using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Games;
using AutoMapper;
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
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetAllGamesUseCase _getAllGamesUseCase;

        public GetAllGamesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _ = _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _getAllGamesUseCase = new GetAllGamesUseCase(_unitOfWorkMock.Object, _mapper, _cacheMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGamesAreFoundInCache()
        {
            var gameDtos = new List<GameDto>
            {
                new GameDto { Id = Guid.NewGuid(), Name = "Game 1", Description = "Description 1", ImageUrl = "image-url-1" },
                new GameDto { Id = Guid.NewGuid(), Name = "Game 2", Description = "Description 2", ImageUrl = "image-url-2" }
            };

            _ = _cacheMock.Setup(c => c.GetAsync<IEnumerable<GameDto>>(It.IsAny<string>())).ReturnsAsync(gameDtos);

            var result = await _getAllGamesUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameDtos.Count, result.Value.Count());
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGamesAreFoundInRepository()
        {
            var games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "Game 1", Description = "Description 1", ImageUrl = "image-url-1" },
                new Game { Id = Guid.NewGuid(), Name = "Game 2", Description = "Description 2", ImageUrl = "image-url-2" }
            };

            _ = _cacheMock.Setup(c => c.GetAsync<IEnumerable<GameDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<GameDto>?)null);
            _ = _gameRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(games);

            var result = await _getAllGamesUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(games.Count, result.Value.Count());
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoGamesFound()
        {
            var games = new List<Game> { };

            _ = _cacheMock.Setup(c => c.GetAsync<IEnumerable<GameDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<GameDto>?)null);
            _ = _gameRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(games);

            var result = await _getAllGamesUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoGamesFound, result.Error);
        }
    }
}