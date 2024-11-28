using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Games.GameOptions
{
    public class CreateGroupedGameOptionsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IRepository<GameKnowledgeSubscription>> _gameKnowledgeSubscriptionRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateGroupedGameOptionsUseCase _createGroupedGameOptionsUseCase;

        public CreateGroupedGameOptionsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _gameKnowledgeSubscriptionRepositoryMock = new Mock<IRepository<GameKnowledgeSubscription>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<GameKnowledgeSubscription>()).Returns(_gameKnowledgeSubscriptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _createGroupedGameOptionsUseCase = new CreateGroupedGameOptionsUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new CreateGroupedGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                GroupedGameOptions = new List<GroupedGameOption>
                {
                    new GroupedGameOption { Type = GameOptionType.Question, Value = "Question 1" },
                    new GroupedGameOption { Type = GameOptionType.Answer, Value = "Answer 1", IsCorrect = true }
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameKnowledgeSubscriptionNotFound()
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

            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync((GameKnowledgeSubscription?)null);

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameKnowledgeSubscriptionNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
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
            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = Guid.NewGuid(),
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);

            var result = await _createGroupedGameOptionsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
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
            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);

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
            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);

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
            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);

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
            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);

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