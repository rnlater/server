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
    public class DeleteGameOptionUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IRepository<GameKnowledgeSubscription>> _gameKnowledgeSubscriptionRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteGameOptionUseCase _deleteGameOptionUseCase;

        public DeleteGameOptionUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>(); _gameKnowledgeSubscriptionRepositoryMock = new Mock<IRepository<GameKnowledgeSubscription>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<GameKnowledgeSubscription>()).Returns(_gameKnowledgeSubscriptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _deleteGameOptionUseCase = new DeleteGameOptionUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var gameOptionId = Guid.NewGuid();


            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionNotFound()
        {
            var gameOptionId = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync((GameOption?)null);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionNotFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameKnowledgeSubscriptionNotFound()
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

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync((GameKnowledgeSubscription?)null);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameKnowledgeSubscriptionNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
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

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = gameOption.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = Guid.NewGuid(),
                    Title = "Knowledge 1"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionGroupNotFound()
        {
            var gameOptionId = Guid.NewGuid();

            var gameOption = new GameOption
            {
                Id = gameOptionId,
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Type = GameOptionType.Answer,
                IsCorrect = true,
                Value = "",
                Group = 2
            };

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = gameOption.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId,
                    Group = 1,
                    Order = 1,
                    Value = "",
                    IsCorrect = true }
                ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionGroupNotFound, result.Error);
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

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = gameOption.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId,
                    Group = gameOption.Group,
                    Order = 1,
                    Value = "",
                    IsCorrect = true }
                ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

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
                Value = "",
                Group = 1
            };

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = gameOption.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId,
                    Group = gameOption.Group,
                    Order = 1,
                    Value = "",
                    IsCorrect = true }
                ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

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
                Value = "",
                Group = 1
            };

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = gameOption.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
            [
                gameOption,
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Question, IsCorrect = false, Group = 1, Value = "" },
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Answer, IsCorrect = false, Group = 1, Value = "" },
                new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = gameOption.GameKnowledgeSubscriptionId, Type = GameOptionType.Answer, IsCorrect = true, Group = 1, Value = "" }
            ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameOptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(gameOption);
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameOptionRepositoryMock.Setup(r => r.Delete(gameOption.Id)).ReturnsAsync(gameOption);

            var result = await _deleteGameOptionUseCase.Execute(gameOptionId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameOptionId, result.Value.Id);
        }
    }
}