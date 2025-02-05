using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.LearningLists;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.LearningLists
{
    public class GetAllLearningListsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<LearningList>> _learningListRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<LearningListKnowledge>> _learningListKnowledgeRepositoryMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly GetAllLearningListsUseCase _getAllLearningListsUseCase;

        public GetAllLearningListsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _learningListRepositoryMock = new Mock<IRepository<LearningList>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _learningListKnowledgeRepositoryMock = new Mock<IRepository<LearningListKnowledge>>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<LearningList>()).Returns(_learningListRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningListKnowledge>()).Returns(_learningListKnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);

            _getAllLearningListsUseCase = new GetAllLearningListsUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _getAllLearningListsUseCase.Execute(new NoParam());

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningListsAreFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var learningLists = new List<LearningList>
            {
                new LearningList { Id = Guid.NewGuid(), Title = "Learning List 1", LearnerId = userId },
                new LearningList { Id = Guid.NewGuid(), Title = "Learning List 2", LearnerId = userId }
            };
            var learningListKnowledge = new List<LearningListKnowledge>
            {
                new LearningListKnowledge { LearningListId = learningLists[0].Id, KnowledgeId = Guid.NewGuid() },
                new LearningListKnowledge { LearningListId = learningLists[1].Id, KnowledgeId = Guid.NewGuid() }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "" });
            _learningListRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(learningLists);
            _learningListKnowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<LearningListKnowledge>>())).ReturnsAsync(learningListKnowledge);
            _learningRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(1);

            // Act
            var result = await _getAllLearningListsUseCase.Execute(new NoParam());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(learningLists.Count, result.Value.Count());
        }
    }
}