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
    public class DeleteGameOptionUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteGameOptionUseCase _deleteGameOptionUseCase;

        public DeleteGameOptionUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);

            _deleteGameOptionUseCase = new DeleteGameOptionUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionNotFound()
        {
            var gameOptionId = Guid.NewGuid();

            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync((GameOption?)null);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionNotFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenDeletingCorrectAnswer()
        {
            var gameOptionId = Guid.NewGuid();
            var gameOption = new GameOption
            {
                Id = gameOptionId,
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Type = GameOptionType.Answer,
                IsCorrect = true,
                Value = "",
                Group = 1
            };

            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.CannotDeleteCorrectAnswer, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenLessThanTwoAnswersRemain()
        {
            var gameOptionId = Guid.NewGuid();
            var gameOption = new GameOption
            {
                Id = gameOptionId,
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Type = GameOptionType.Answer,
                IsCorrect = false,
                Group = 1,
                Value = ""
            };

            var gameOptions = new List<GameOption>
            {
                gameOption,
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Answer, IsCorrect = false, Group = 1, Value = "" }
            };

            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOptions);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireAtLeastTwoAnswers, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameOptionIsDeleted()
        {
            var gameOptionId = Guid.NewGuid();
            var gameOption = new GameOption
            {
                Id = gameOptionId,
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Type = GameOptionType.Answer,
                IsCorrect = false,
                Group = 1,
                Value = ""
            };

            var gameOptions = new List<GameOption>
            {
                gameOption,
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Question, IsCorrect = false, Group = 1, Value = "" },
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Answer, IsCorrect = false, Group = 1, Value = "" },
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Answer, IsCorrect = true, Group = 1, Value = "" }
            };

            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOptions);
            _gameOptionRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(gameOption);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameOptionId, result.Value.Id);
        }

    }
}