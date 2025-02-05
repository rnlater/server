using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Games.GameOptions
{
    public class UpdateGameOptionUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameKnowledgeSubscription>> _gameKnowledgeSubscriptionRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly UpdateGameOptionUseCase _updateGameOptionUseCase;

        public UpdateGameOptionUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _gameKnowledgeSubscriptionRepositoryMock = new Mock<IRepository<GameKnowledgeSubscription>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<GameKnowledgeSubscription>()).Returns(_gameKnowledgeSubscriptionRepositoryMock.Object);

            _updateGameOptionUseCase = new UpdateGameOptionUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new UpdateGameOptionParams
            {
                Id = Guid.NewGuid(),
                Value = "Updated Answer",
                Group = 1,
                IsCorrect = true
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _updateGameOptionUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameKnowledgeSubscriptionNotFound()
        {
            var parameters = new UpdateGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 1,
                IsCorrect = true
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync((GameKnowledgeSubscription?)null);

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameKnowledgeSubscriptionNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
        {
            var parameters = new UpdateGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 1,
                IsCorrect = true
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
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionGroupNotFound()
        {
            var parameters = new UpdateGameOptionParams
            {
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 2,
                IsCorrect = true
            };


            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption { Id = Guid.NewGuid(), GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                    Group = 1,
                    Order = 1,
                    Value = "",
                    IsCorrect = true }
                ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionGroupNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenGameOptionNotFoundWithGuid()
        {
            var parameters = new UpdateGameOptionParams
            {
                Id = Guid.NewGuid(),
                GameKnowledgeSubscriptionId = Guid.NewGuid(),
                Value = "Answer 1",
                Group = 1,
                IsCorrect = true
            };

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption {
                        Id = Guid.NewGuid(),
                        GameKnowledgeSubscriptionId = parameters.GameKnowledgeSubscriptionId,
                        Group = parameters.Group,
                        Order = 1,
                        Value = "",
                        IsCorrect = true
                    }
                ]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());

            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.GameOptionNotFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenGameOptionIsUpdated()
        {
            var gameOptionId = Guid.NewGuid();
            var parameters = new UpdateGameOptionParams
            {
                Id = gameOptionId,
                Value = "Updated Answer",
                Group = 1,
                IsCorrect = false
            };

            var gameKnowledgeSubscription = new GameKnowledgeSubscription
            {
                Id = parameters.GameKnowledgeSubscriptionId,
                Knowledge = new Knowledge
                {
                    CreatorId = SeedData.GetUsers().First().Id,
                    Title = "Knowledge 1"
                },
                GameOptions =
                [
                    new GameOption
                    {
                        Id = gameOptionId,
                        GameKnowledgeSubscriptionId = Guid.NewGuid(),
                        Value = "Old Answer",
                        Group = parameters.Group,
                        IsCorrect = true
                    }
                ]
            };


            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", SeedData.GetUsers().First().Id.ToString()));
            _gameKnowledgeSubscriptionRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<GameKnowledgeSubscription>>())).ReturnsAsync(gameKnowledgeSubscription);
            _userRepositoryMock.Setup(r => r.GetById(SeedData.GetUsers().First().Id)).ReturnsAsync(SeedData.GetUsers().First());
            _gameOptionRepositoryMock.Setup(r => r.Update(It.IsAny<GameOption>())).ReturnsAsync(new GameOption
            {
                Id = gameOptionId,
                Value = parameters.Value,
                Group = parameters.Group,
                IsCorrect = parameters.IsCorrect
            });


            var result = await _updateGameOptionUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(gameOptionId, result.Value.Id);
            Assert.Equal(parameters.Value, result.Value.Value);
        }
    }
}