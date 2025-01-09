using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.Learnings;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.Learnings
{
    public class GetCurrentUserLearningsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Learning>> _learningRepositoryMock;
        private readonly Mock<IRepository<LearningList>> _learningListRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly GetCurrentUserLearningsUseCase _getCurrentUserLearningsUseCase;

        public GetCurrentUserLearningsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _learningRepositoryMock = new Mock<IRepository<Learning>>();
            _learningListRepositoryMock = new Mock<IRepository<LearningList>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Learning>()).Returns(_learningRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<LearningList>()).Returns(_learningListRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _getCurrentUserLearningsUseCase = new GetCurrentUserLearningsUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new GetCurrentUserLearningsParams();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            var result = await _getCurrentUserLearningsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLearningsAreFound()
        {
            var userId = Guid.NewGuid();
            var parameters = new GetCurrentUserLearningsParams();

            var learnings = new List<Learning>
            {
                new Learning { Id = Guid.NewGuid(), UserId = userId, KnowledgeId = Guid.NewGuid() },
                new Learning { Id = Guid.NewGuid(), UserId = userId, KnowledgeId = Guid.NewGuid() }
            };

            var learningLists = new List<LearningList>
            {
                new LearningList
                {
                    Id = Guid.NewGuid(),
                    Title = "Title",
                    LearnerId = userId,
                    LearningListKnowledges = new List<LearningListKnowledge>
                    {
                        new LearningListKnowledge { LearningListId = Guid.NewGuid(), KnowledgeId = Guid.NewGuid() }
                    }
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "Email", UserName = "UserName" });
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(learnings);
            _learningListRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(learningLists);

            var result = await _getCurrentUserLearningsUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenNoLearningsFound()
        {
            var userId = Guid.NewGuid();
            var parameters = new GetCurrentUserLearningsParams
            {
                Search = "Search"
            };

            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "Email", UserName = "UserName" });
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _learningRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Learning>>())).ReturnsAsync(new List<Learning>());
            _learningListRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<LearningList>>())).ReturnsAsync(new List<LearningList>());

            var result = await _getCurrentUserLearningsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoData, result.Error);
        }
    }
}