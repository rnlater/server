using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Games;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Shared.Types;

namespace UnitTests.Games
{
    public class DeleteGameUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly DeleteGameUseCase _deleteGameUseCase;

        public DeleteGameUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _deleteGameUseCase = new DeleteGameUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameNotFound()
        {
            var gameId = Guid.NewGuid();

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync((Game?)null);

            var result = await _deleteGameUseCase.Execute(gameId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoGameFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameIsDeleted()
        {
            var gameId = Guid.NewGuid();
            var game = new Game { Id = gameId, Name = "Test Game", ImageUrl = "test-image-url", Description = "" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);
            _gameRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(game);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Done("test-image-url"));

            var result = await _deleteGameUseCase.Execute(gameId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameId, result.Value.Id);
            Assert.Equal(game.Name, result.Value.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenFileDeletionFails()
        {
            var gameId = Guid.NewGuid();
            var game = new Game { Id = gameId, Name = "Test Game", ImageUrl = "test-image-url", Description = "" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);
            _gameRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(game);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Fail(ErrorMessage.DeleteFileError));

            var result = await _deleteGameUseCase.Execute(gameId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.DeleteFileError, result.Error);
        }

    }
}