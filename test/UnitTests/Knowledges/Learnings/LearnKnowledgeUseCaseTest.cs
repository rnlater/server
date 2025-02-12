using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.Learnings;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
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
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<GameOption>> _gameOptionRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IRepository<LearningHistory>> _learningHistoryRepositoryMock;
        private readonly Mock<IRepository<LearningListKnowledge>> _learningListKnowledgeRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly LearnKnowledgeUseCase _learnKnowledgeUseCase;

        public LearnKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _gameOptionRepositoryMock = new Mock<IRepository<GameOption>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _learningHistoryRepositoryMock = new Mock<IRepository<LearningHistory>>();
            _learningListKnowledgeRepositoryMock = new Mock<IRepository<LearningListKnowledge>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<GameOption>()).Returns(_gameOptionRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningHistory>()).Returns(_learningHistoryRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningListKnowledge>()).Returns(_learningListKnowledgeRepositoryMock.Object);

            _learnKnowledgeUseCase = new LearnKnowledgeUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    QuestionIdOne = Guid.NewGuid(),
                    AnswerOne = "AnswerOne",
                    QuestionIdTwo = Guid.NewGuid(),
                    AnswerTwo = "AnswerTwo",
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)).Returns((Claim?)null);

            var result = await _learnKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgesNotFound()
        {
            var userId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    QuestionIdOne = Guid.NewGuid(),
                    AnswerOne = "AnswerOne",
                    QuestionIdTwo = Guid.NewGuid(),
                    AnswerTwo = "AnswerTwo",
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count - 1);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());

            var result = await _learnKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgesNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgesAlreadyLearned()
        {
            var userId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = Guid.NewGuid(),
                    QuestionIdOne = Guid.NewGuid(),
                    AnswerOne = "AnswerOne",
                    QuestionIdTwo = Guid.NewGuid(),
                    AnswerTwo = "AnswerTwo",
                    Interpretation = "Interpretation",
                    WordMatchAnswer = "Answer"
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });
            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(parameters.Count);

            var result = await _learnKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgesAlreadyLearned, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenInvalidData()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = knowledgeId,
                    QuestionIdOne = Guid.NewGuid(),
                    AnswerOne = "Invalid Answer",
                    QuestionIdTwo = Guid.NewGuid(),
                    AnswerTwo = "AnswerTwo",
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

            var correctGameOptionOne = new GameOption
            {
                Id = parameters[0].QuestionIdOne,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge
                }
            };

            var correctGameOptionTwo = new GameOption
            {
                Id = parameters[0].QuestionIdTwo,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "Email", UserName = "UserName" });

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());

            _gameOptionRepositoryMock.Setup(r => r.Find(It.Is<BaseSpecification<GameOption>>(spec => spec.Criteria!.Compile()(correctGameOptionOne)))).ReturnsAsync(correctGameOptionOne);
            _gameOptionRepositoryMock.Setup(r => r.Find(It.Is<BaseSpecification<GameOption>>(spec => spec.Criteria!.Compile()(correctGameOptionTwo)))).ReturnsAsync(correctGameOptionTwo);

            var result = await _learnKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.InvalidData, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningIsCreated()
        {
            var userId = Guid.NewGuid();
            var knowledgeId = Guid.NewGuid();
            var parameters = new List<LearnKnowledgeParams>
            {
                new LearnKnowledgeParams
                {
                    KnowledgeId = knowledgeId,
                    QuestionIdOne = Guid.NewGuid(),
                    AnswerOne = "AnswerOne",
                    QuestionIdTwo = Guid.NewGuid(),
                    AnswerTwo = "AnswerTwo",
                    Interpretation = "Interpretation",
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
            var correctGameOptionOne = new GameOption
            {
                Id = parameters[0].QuestionIdOne,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge,
                    KnowledgeId = knowledge.Id
                }
            };

            var correctGameOptionTwo = new GameOption
            {
                Id = parameters[0].QuestionIdTwo,
                Value = "",
                GameKnowledgeSubscription = new GameKnowledgeSubscription
                {
                    GameId = Guid.NewGuid(),
                    Knowledge = knowledge,
                    KnowledgeId = knowledge.Id
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "Email", UserName = "UserName" });

            _knowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(parameters.Count);
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());

            _gameOptionRepositoryMock.Setup(r => r.Find(It.Is<BaseSpecification<GameOption>>(spec => spec.Criteria!.Compile()(correctGameOptionOne)))).ReturnsAsync(correctGameOptionOne);
            _gameOptionRepositoryMock.Setup(r => r.Find(It.Is<BaseSpecification<GameOption>>(spec => spec.Criteria!.Compile()(correctGameOptionTwo)))).ReturnsAsync(correctGameOptionTwo);

            _learningRepositoryMock.Setup(r => r.Add(It.IsAny<Learning>())).ReturnsAsync(new Learning { Id = Guid.NewGuid(), KnowledgeId = knowledgeId, UserId = userId });
            _learningHistoryRepositoryMock.Setup(r => r.Add(It.IsAny<LearningHistory>())).ReturnsAsync(new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne });

            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new Learning
            {
                Id = Guid.NewGuid(),
                Knowledge = knowledge,
                LearningHistories = new List<LearningHistory>
                {
                    new LearningHistory { Id = Guid.NewGuid(), LearningId = Guid.NewGuid(), LearningLevel = LearningLevel.LevelOne }
                }
            });
            _learningListKnowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(0);

            var result = await _learnKnowledgeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
        }
    }
}
