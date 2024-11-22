using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.LearningLists;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.LearningLists
{
    public class AddRemoveKnowledgeToLearningListUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<LearningListKnowledge>> _learningListKnowledgeRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _KnowledgeRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly AddRemoveKnowledgeToLearningListUseCase _addRemoveKnowledgeToLearningListUseCase;

        public AddRemoveKnowledgeToLearningListUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _learningListKnowledgeRepositoryMock = new Mock<IRepository<LearningListKnowledge>>();
            _KnowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<LearningListKnowledge>()).Returns(_learningListKnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_KnowledgeRepositoryMock.Object);

            _addRemoveKnowledgeToLearningListUseCase = new AddRemoveKnowledgeToLearningListUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgeToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _addRemoveKnowledgeToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotLearner()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgeToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeId,
                LearningList = new LearningList
                {
                    Title = "",
                    LearnerId = Guid.NewGuid()
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(learningListKnowledge);

            // Act
            var result = await _addRemoveKnowledgeToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotCreatorOfPrivateKnowledge()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgeToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();

            var knowledge = new Knowledge
            {
                Title = "",
                CreatorId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Private
            };


            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync((LearningListKnowledge?)null);
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(knowledge);

            // Act
            var result = await _addRemoveKnowledgeToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsRemovedFromLearningList()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgeToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeId,
                LearningList = new LearningList
                {
                    Title = "",
                    LearnerId = userId
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(learningListKnowledge);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Delete(It.IsAny<LearningListKnowledge>())).ReturnsAsync(learningListKnowledge);

            // Act
            var result = await _addRemoveKnowledgeToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningListKnowledge.LearningListId, result.Value.LearningListId);
            Assert.Equal(learningListKnowledge.KnowledgeId, result.Value.KnowledgeId);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsAddedToLearningList()
        {
            // Arrange
            var parameters = new AddRemoveKnowledgeToLearningListParams
            {
                LearningListId = Guid.NewGuid(),
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var learningListKnowledge = new LearningListKnowledge
            {
                LearningListId = parameters.LearningListId,
                KnowledgeId = parameters.KnowledgeId,
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

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync((LearningListKnowledge?)null);
            _learningListKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<LearningListKnowledge>())).ReturnsAsync(learningListKnowledge);
            _KnowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(learningListKnowledge.Knowledge);

            // Act
            var result = await _addRemoveKnowledgeToLearningListUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningListKnowledge.LearningListId, result.Value.LearningListId);
            Assert.Equal(learningListKnowledge.KnowledgeId, result.Value.KnowledgeId);
        }
    }
}