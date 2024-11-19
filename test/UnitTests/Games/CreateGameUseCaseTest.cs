using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Games;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;
using Shared.Types;

namespace UnitTests.Games
{
    public class CreateGameUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Game>> _gameRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly CreateGameUseCase _createGameUseCase;

        public CreateGameUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameRepositoryMock = new Mock<IRepository<Game>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);

            _createGameUseCase = new CreateGameUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameIsCreated()
        {
            var parameters = new CreateGameParams
            {
                Name = "Test Game",
                Description = "Test Description",
                Image = new Mock<IFormFile>().Object
            };

            var game = new Game { Id = Guid.NewGuid(), Name = parameters.Name, Description = parameters.Description, ImageUrl = "test-image-url" };

            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Done("test-image-url"));
            _gameRepositoryMock.Setup(r => r.Add(It.IsAny<Game>())).ReturnsAsync(game);

            var result = await _createGameUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(game.Id, result.Value.Id);
            Assert.Equal(game.Name, result.Value.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenFileStorageFails()
        {
            var parameters = new CreateGameParams
            {
                Name = "Test Game",
                Description = "Test Description",
                Image = new Mock<IFormFile>().Object
            };

            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync(Result<string>.Fail(ErrorMessage.StoreFileError));

            var result = await _createGameUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.StoreFileError, result.Error);
        }
    }
}