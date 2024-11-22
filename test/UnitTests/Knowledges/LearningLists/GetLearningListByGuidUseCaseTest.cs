using System.Security.Claims;
using Application.UseCases.Knowledges.LearningLists;
using AutoMapper;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;
using Application.Mappings;
using Domain.Base;


namespace UnitTests.Knowledges.LearningLists
{
    public class GetLearningListByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<LearningList>> _learningListRepositoryMock;
        private readonly Mock<IRepository<LearningListKnowledge>> _learningListKnowledgeRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly GetLearningListByGuidUseCase _getLearningListByGuidUseCase;

        public GetLearningListByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _learningListRepositoryMock = new Mock<IRepository<LearningList>>();
            _learningListKnowledgeRepositoryMock = new Mock<IRepository<LearningListKnowledge>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<LearningList>()).Returns(_learningListRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningListKnowledge>()).Returns(_learningListKnowledgeRepositoryMock.Object);

            _getLearningListByGuidUseCase = new GetLearningListByGuidUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var learningList = new LearningList
            {
                Id = guid,
                Title = "Test Learning List",
                LearnerId = Guid.NewGuid()
            };

            _learningListRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(learningList);
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _getLearningListByGuidUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenLearningListNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync((LearningList?)null);

            // Act
            var result = await _getLearningListByGuidUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoLearningListFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var learningList = new LearningList
            {
                Id = guid,
                Title = "Test Learning List",
                LearnerId = Guid.NewGuid()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(learningList);

            // Act
            var result = await _getLearningListByGuidUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningListIsFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var learningList = new LearningList
            {
                Id = guid,
                Title = "Test Learning List",
                LearnerId = userId,
                LearningListKnowledges = new List<LearningListKnowledge>
                {
                    new LearningListKnowledge
                    {
                        LearningListId = guid,
                        KnowledgeId = Guid.NewGuid(),
                        Knowledge = new Knowledge
                        {
                            Id = Guid.NewGuid(),
                            Title = "Test Knowledge",
                            Visibility = KnowledgeVisibility.Public,
                            CreatorId = userId
                        }
                    }
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(learningList);

            // Act
            var result = await _getLearningListByGuidUseCase.Execute(guid);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningList.Id, result.Value.Id);
            Assert.Equal(learningList.Title, result.Value.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningListRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<LearningList>>())).ThrowsAsync(new Exception());

            // Act
            var result = await _getLearningListByGuidUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError, result.Error);
        }
    }
}