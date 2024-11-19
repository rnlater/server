using Application.Mappings;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Games.GameOptions
{
    public class CreateGroupedGameOptionsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateGroupedGameOptionsUseCase _createGroupedGameOptionsUseCase;

        public CreateGroupedGameOptionsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);

            _createGroupedGameOptionsUseCase = new CreateGroupedGameOptionsUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoQuestionOptionProvided()
        {
            var parameters = new CreateGroupedGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                GroupedGameOptions = new List<GroupedGameOption>
                {
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 2", IsCorrect = false }
                }
            };

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireExactOneQuestion, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenLessThanTwoAnswerOptionsProvided()
        {
            var parameters = new CreateGroupedGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                GroupedGameOptions = new List<GroupedGameOption>
                {
                    new GroupedGameOption { Type = GameOptionType.Question, Value = "Question 1" },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true }
                }
            };

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireAtLeastTwoAnswers, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenMoreThanOneCorrectAnswerProvided()
        {
            var parameters = new CreateGroupedGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                GroupedGameOptions = new List<GroupedGameOption>
                {
                    new GroupedGameOption { Type = GameOptionType.Question, Value = "Question 1" },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 2", IsCorrect = true }
                }
            };

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireExactOneCorrectAnswer, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGroupedGameOptionsAreCreated()
        {
            var parameters = new CreateGroupedGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                GroupedGameOptions = new List<GroupedGameOption>
                {
                    new GroupedGameOption { Type = GameOptionType.Question, Value = "Question 1" },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 2", IsCorrect = false }
                }
            };

            var gameOptions = parameters.GroupedGameOptions.Select(go => new GameOption
            {
                Id = Guid.NewGuid(),
                GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                Type = go.Type,
                Value = go.Value,
                IsCorrect = go.IsCorrect
            }).ToList();

            foreach (var gameOption in gameOptions)
            {
                _gameOptionRepositoryMock.Setup(r => r.Add(It.IsAny<GameOption>())).ReturnsAsync(gameOption);
            }

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameOptions.Count, result.Value.Count);
        }
    }
}