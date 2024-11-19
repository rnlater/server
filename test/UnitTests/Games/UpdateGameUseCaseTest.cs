using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Games;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;
using Shared.Types;

namespace UnitTests.Games
{
    public class UpdateGameUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly UpdateGameUseCase _updateGameUseCase;

        public UpdateGameUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _updateGameUseCase = new UpdateGameUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameNotFound()
        {
            // Arrange
            var parameters = new UpdateGameParams
            {
                Id = Guid.NewGuid(),
                Name = "Updated Game",
                Description = "Updated Description",
                Image = new Mock<IFormFile>().Object
            };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync((Game?)null);

            // Act
            var result = await _updateGameUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoGameFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameIsUpdated()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var parameters = new UpdateGameParams
            {
                Id = gameId,
                Name = "Updated Game",
                Description = "Updated Description",
                Image = new Mock<IFormFile>().Object
            };

            var game = new Game { Id = gameId, Name = "Old Game", Description = "Old Description", ImageUrl = "old-image-url" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Done("old-image-url"));
            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Done("new-image-url"));
            _gameRepositoryMock.Setup(r => r.Update(It.IsAny<Game>())).ReturnsAsync(game);

            // Act
            var result = await _updateGameUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameId, result.Value.Id);
            Assert.Equal(parameters.Name, result.Value.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenFileStorageFails()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var parameters = new UpdateGameParams
            {
                Id = gameId,
                Name = "Updated Game",
                Description = "Updated Description",
                Image = new Mock<IFormFile>().Object
            };

            var game = new Game { Id = gameId, Name = "Old Game", Description = "Old Description", ImageUrl = "old-image-url" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Done("old-image-url"));
            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Fail(ErrorMessage.StoreFileError));

            // Act
            var result = await _updateGameUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.StoreFileError, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenFileDeletionFails()
        {
            // Arrange
            var gameId = Guid.NewGuid();
            var parameters = new UpdateGameParams
            {
                Id = gameId,
                Name = "Updated Game",
                Description = "Updated Description",
                Image = new Mock<IFormFile>().Object
            };

            var game = new Game { Id = gameId, Name = "Old Game", Description = "Old Description", ImageUrl = "old-image-url" };

            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync(game);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Fail(ErrorMessage.DeleteFileError));

            // Act
            var result = await _updateGameUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.DeleteFileError, result.Error);
        }
    }
}