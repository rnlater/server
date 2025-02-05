using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.LearningLists;
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

namespace UnitTests.Knowledges.LearningLists
{
    public class AddRemoveKnowledgesToLearningListUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<LearningListKnowledge>> _learningListKnowledgeRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _KnowledgeRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly AddRemoveKnowledgesToLearningListUseCase _AddRemoveKnowledgesToLearningListUseCase;

        public AddRemoveKnowledgesToLearningListUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _learningListKnowledgeRepositoryMock = new Mock<IRepository<LearningListKnowledge>>();
            _KnowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<LearningListKnowledge>()).Returns(_learningListKnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_KnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);

            _AddRemoveKnowledgesToLearningListUseCase = new AddRemoveKnowledgesToLearningListUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = true
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeFoundWithGuid()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = true
            };

            var userId = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotLearner()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = true
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeIds.First(),
                LearningList = new LearningList
                {
                    Title = "",
                    LearnerId = Guid.NewGuid()
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new Knowledge { Title = "", CreatorId = Guid.NewGuid(), Visibility = KnowledgeVisibility.Private });
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(learningListKnowledge);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotCreatorOfPrivateKnowledge()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = true
            };

            var userId = Guid.NewGuid();

            var knowledge = new Knowledge
            {
                Title = "",
                CreatorId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Private
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = Guid.NewGuid(), Email = "", UserName = "" });
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync((LearningListKnowledge?)null);
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(knowledge);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsRemovedFromLearningList()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = false
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeIds.First(),
                LearningList = new LearningList
                {
                    Title = "",
                    LearnerId = userId
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "" });
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new Knowledge { Title = "", CreatorId = userId });
            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync((Learning?)null);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(learningListKnowledge);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Delete(It.IsAny<LearningListKnowledge>())).ReturnsAsync(learningListKnowledge);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningListKnowledge.LearningListId, result.Value.First().LearningListId);
            Assert.Equal(learningListKnowledge.KnowledgeId, result.Value.First().KnowledgeId);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsAddedToLearningList()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgesToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeIds = new List<Guid> { Guid.NewGuid() },
                IsAdd = true
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeIds.First(),
                LearningList = new LearningList
                {
                    Title = "",
                    LearnerId = userId
                },
                Knowledge = new Knowledge
                {
                    Title = "",
                    CreatorId = userId,
                    Visibility = KnowledgeVisibility.Private
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "" });
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(learningListKnowledge.Knowledge);
            _learningRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync((Learning?)null);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync((LearningListKnowledge?)null);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<LearningListKnowledge>())).ReturnsAsync(learningListKnowledge);

            // Act
            var result = await _AddRemoveKnowledgesToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningListKnowledge.LearningListId, result.Value.First().LearningListId);
            Assert.Equal(learningListKnowledge.KnowledgeId, result.Value.First().KnowledgeId);
        }
    }
}