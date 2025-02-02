using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.Learnings;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.Learnings
{
    public class LearnKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IRepository<LearningHistory>> _learningHistoryRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly LearnKnowledgeUseCase _learnKnowledgeUseCase;

        public LearnKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _learningHistoryRepositoryMock = new Mock<IRepository<LearningHistory>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningHistory>()).Returns(_learningHistoryRepositoryMock.Object);

            _learnKnowledgeUseCase = new LearnKnowledgeUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync([]);
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)).Returns((Claim?)null);

            // Act
            var result = await _learnKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgesNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count - 1);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync([]);

            // Act
            var result = await _learnKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgesNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgesAlreadyLearned()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync([
                new Learning { KnowledgeId = parameters[0].KnowledgeId }
            ]);

            // Act
            var result = await _learnKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgesAlreadyLearned, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenInvalidData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = knowledgeId,
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Invalid Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            var knowledge = new Knowledge
            {
                Id = knowledgeId,
                Title = "Knowledge 1",
                Materials = new List<Material>
                {
                    new Material { Id = Guid.NewGuid(), Content = "Interpretation", Type = MaterialType.Interpretation },
                    new Material { Id = Guid.NewGuid(), Content = "", Type = MaterialType.TextMedium }
                }
            };

            var correctGameOption = new GameOption
            {
                Id = parameters[0].CorrectGameOptionId,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(new List<GameOption> { correctGameOption, new GameOption { Value = "" } });

            // Act
            var result = await _learnKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.InvalidData, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningIsCreated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = knowledgeId,
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            var knowledge = new Knowledge
            {
                Id = knowledgeId,
                Title = "Knowledge 1",
                Materials =
                [
                    new Material { Id = Guid.NewGuid(), Content = "Interpretation", Type = MaterialType.Interpretation },
                    new Material { Id = Guid.NewGuid(), Content = "", Type = MaterialType.TextMedium }
                ]
            };
            var correctGameOption = new GameOption
            {
                Id = parameters[0].CorrectGameOptionId,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync([]);
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync([correctGameOption, new GameOption() { Value = "" }]);

            _learningRepositoryMock.Setup(r => r.Add(It.IsAny<Learning>())).ReturnsAsync(new Learning { Id = Guid.NewGuid(), KnowledgeId = knowledgeId, UserId = userId });
            _learningHistoryRepositoryMock.Setup(r => r.Add(It.IsAny<LearningHistory>())).ReturnsAsync(new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne });

            // Act
            var result = await _learnKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
            Assert.Equal(result.Value.ToList().First().Key, knowledgeId);
            Assert.Equal(0, result.Value.ToList().First().Value);
        }

    }
}