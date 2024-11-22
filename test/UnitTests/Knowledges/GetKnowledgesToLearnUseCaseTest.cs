using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class GetKnowledgesToLearnUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly IMapper _mapper;

        private readonly GetKnowledgesToLearnUseCase _getKnowledgesToLearnUseCase;

        public GetKnowledgesToLearnUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);

            _getKnowledgesToLearnUseCase = new GetKnowledgesToLearnUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new GetKnowledgesToLearnParams
            {
                KnowledgeIds = [Guid.NewGuid()]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)).Returns((Claim?)null);

            var result = await _getKnowledgesToLearnUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeAlreadyLearned()
        {
            var userId = Guid.NewGuid();
            var parameters = new GetKnowledgesToLearnParams
            {
                KnowledgeIds = [Guid.NewGuid()]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });

            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(1);

            var result = await _getKnowledgesToLearnUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.KnowledgeAlreadyLearned, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgesFound()
        {
            var userId = Guid.NewGuid();
            var parameters = new GetKnowledgesToLearnParams
            {
                KnowledgeIds = [Guid.NewGuid()]
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });

            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(0);
            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(Enumerable.Empty<Knowledge>());

            var result = await _getKnowledgesToLearnUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgesNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeRequireAGameToReview()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new GetKnowledgesToLearnParams
            {
                KnowledgeIds = [knowledgeId]
            };

            var knowledges = new List<Knowledge>
            {
                new Knowledge
                {
                    Id = knowledgeId,
                    Title = "Knowledge 1",
                    GameKnowledgeSubscriptions =
                    [],
                    Materials =
                    [
                        new Material { Type = MaterialType.Interpretation, Content = "Interpretation 1" },
                        new Material { Type = MaterialType.Interpretation, Content = "Interpretation 2" },
                        new Material { Type = MaterialType.TextMedium, Content = "Medium Text 3" }
                    ]
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));

            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(0);
            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _getKnowledgesToLearnUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RequireAGameToReview, result.Error);
        }


        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgesAreFound()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new GetKnowledgesToLearnParams
            {
                KnowledgeIds = [knowledgeId]
            };

            var knowledges = new List<Knowledge>
            {
                new Knowledge
                {
                    Id = knowledgeId,
                    Title = "Knowledge 1",
                    GameKnowledgeSubscriptions =
                    [
                        new GameKnowledgeSubscription
                        {
                            GameId = Guid.NewGuid(),
                            KnowledgeId = knowledgeId,
                            GameOptions =
                            [
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 1", IsCorrect = false, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 2", IsCorrect = true, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 3", IsCorrect = false, Group = 2 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 4", IsCorrect = true, Group = 2 }
                            ]
                        },
                        new GameKnowledgeSubscription
                        {
                            GameId = Guid.NewGuid(),
                            KnowledgeId = knowledgeId,
                            GameOptions =
                            [
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 1", IsCorrect = false, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 2", IsCorrect = true, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 3", IsCorrect = false, Group = 2 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 4", IsCorrect = true, Group = 2 }
                            ]
                        },
                        new GameKnowledgeSubscription
                        {
                            GameId = Guid.NewGuid(),
                            KnowledgeId = knowledgeId,
                            GameOptions =
                            [
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 1", IsCorrect = false, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 2", IsCorrect = true, Group = 1 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Question, Value = "Option 3", IsCorrect = false, Group = 2 },
                                new GameOption { Id = Guid.NewGuid(), Type = GameOptionType.Answer, Value = "Option 4", IsCorrect = true, Group = 2 }
                            ]
                        }
                    ],
                    Materials =
                    [
                        new Material { Type = MaterialType.Interpretation, Content = "Interpretation 1" },
                        new Material { Type = MaterialType.Interpretation, Content = "Interpretation 2" },
                        new Material { Type = MaterialType.TextMedium, Content = "Medium Text 3" }
                    ]
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });

            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(0);
            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _getKnowledgesToLearnUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.First());
            Assert.Equal(knowledgeId, result.Value.First().ToList().First().Value.Knowledge.Id);
            Assert.Equal("Knowledge 1", result.Value.First().ToList().First().Value.Knowledge.Title);
            Assert.Equal(3, result.Value.First().ToList().First().Value.Knowledge.Materials.Count);
            Assert.NotNull(result.Value.First().ToList().First().Value.Knowledge.GameToReview);
        }
    }
}