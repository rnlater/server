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
    public class ReviewLearningUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IRepository<LearningHistory>> _learningHistoryRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly ReviewLearningUseCase _reviewLearningUseCase;

        public ReviewLearningUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _learningHistoryRepositoryMock = new Mock<IRepository<LearningHistory>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningHistory>()).Returns(_learningHistoryRepositoryMock.Object);

            _reviewLearningUseCase = new ReviewLearningUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new List<ReviewLearningParams>
            {
                new ReviewLearningParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)).Returns((Claim?)null);

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenLearningNotFound()
        {
            var userId = Guid.NewGuid();
            var parameters = new List<ReviewLearningParams>
            {
                new ReviewLearningParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync((Learning?)null);

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.LearningNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotReadyToReview()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<ReviewLearningParams>
            {
                new() {
                    KnowledgeId = knowledgeId,
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            var learning = new Learning
            {
                KnowledgeId = knowledgeId,
                UserId = userId,
                NextReviewDate = DateTime.Now.AddDays(1),
                LearningHistories = new List<LearningHistory>()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(learning);

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.KnowledgeNotReadyToReview, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenReviewBeforeLearning()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<ReviewLearningParams>
            {
                new ReviewLearningParams
                {
                    KnowledgeId = knowledgeId,
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            var learning = new Learning
            {
                KnowledgeId = knowledgeId,
                UserId = userId,
                LearningHistories = new List<LearningHistory>()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(learning);

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireLearningBeforeReview, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenInvalidData()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<ReviewLearningParams>
            {
                new ReviewLearningParams
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

            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new Learning
            {
                KnowledgeId = knowledgeId,
                UserId = userId,
                LearningHistories = new List<LearningHistory>() {
                    new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne }
                }
            });
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(new List<GameOption> { correctGameOption, new GameOption { Value = "" } });

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.InvalidData, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningIsReviewed()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<ReviewLearningParams>
            {
                new ReviewLearningParams
                {
                    KnowledgeId = knowledgeId,
                    CorrectGameOptionId = Guid.NewGuid(),
                    GameOptionAnswerId = Guid.NewGuid(),
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            var correctGameOption = new GameOption
            {
                Id = parameters[0].CorrectGameOptionId,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = new Knowledge
                    {
                        Id = knowledgeId,
                        Title = "Knowledge 1",
                        Materials = new List<Material>
                            {
                                new Material { Id = Guid.NewGuid(), Content = "Interpretation", Type = MaterialType.Interpretation },
                                new Material { Id = Guid.NewGuid(), Content = "", Type = MaterialType.TextMedium }
                            }
                    }
                }
            };

            var learning = new Learning
            {
                KnowledgeId = knowledgeId,
                UserId = userId,
                LearningHistories = new List<LearningHistory>() {
                    new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne }
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(learning);
            _gameOptionRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<GameOption>>())).ReturnsAsync(new List<GameOption> { correctGameOption, new GameOption { Value = "" } });

            _learningHistoryRepositoryMock.Setup(r => r.Add(It.IsAny<LearningHistory>())).ReturnsAsync(new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne });

            var result = await _reviewLearningUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
        }
    }
}