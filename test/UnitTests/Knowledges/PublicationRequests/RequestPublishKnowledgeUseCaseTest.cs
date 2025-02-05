using System.Security.Claims;
using Application.Mappings;
using Application.UseCases.Knowledges.PublicationRequests;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.PublicationRequests
{
    public class RequestPublishKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<PublicationRequest>> _publicationRequestRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly RequestPublishKnowledgeUseCase _requestPublishKnowledgeUseCase;

        public RequestPublishKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _publicationRequestRepositoryMock = new Mock<IRepository<PublicationRequest>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PublicationRequest>()).Returns(_publicationRequestRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _requestPublishKnowledgeUseCase = new RequestPublishKnowledgeUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var parameters = new RequestPublishKnowledgeParams
            {
                KnowledgeId = Guid.NewGuid()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _requestPublishKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var parameters = new RequestPublishKnowledgeParams
            {
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _requestPublishKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
        {
            // Arrange
            var parameters = new RequestPublishKnowledgeParams
            {
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                CreatorId = Guid.NewGuid()
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);

            // Act
            var result = await _requestPublishKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeAlreadyRequestedForPublication()
        {
            // Arrange
            var parameters = new RequestPublishKnowledgeParams
            {
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                CreatorId = userId,
                PublicationRequest = new PublicationRequest
                {
                    Id = Guid.NewGuid(),
                    Status = PublicationRequestStatus.Pending
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);

            // Act
            var result = await _requestPublishKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.KnowledgeAlreadyRequestedForPublication, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestIsCreated()
        {
            // Arrange
            var parameters = new RequestPublishKnowledgeParams
            {
                KnowledgeId = Guid.NewGuid()
            };

            var userId = Guid.NewGuid();
            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                CreatorId = userId
            };

            var publicationRequest = new PublicationRequest
            {
                Id = Guid.NewGuid(),
                KnowledgeId = parameters.KnowledgeId
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _publicationRequestRepositoryMock.Setup(r => r.Add(It.IsAny<PublicationRequest>())).ReturnsAsync(publicationRequest);

            // Act
            var result = await _requestPublishKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(publicationRequest.Id, result.Value.Id);
        }
    }
}