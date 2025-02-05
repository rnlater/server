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
    public class DeletePublicationRequestUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<PublicationRequest>> _publicationRequestRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly DeletePublicationRequestUseCase _deletePublicationRequestUseCase;

        public DeletePublicationRequestUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _publicationRequestRepositoryMock = new Mock<IRepository<PublicationRequest>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<PublicationRequest>()).Returns(_publicationRequestRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _deletePublicationRequestUseCase = new DeletePublicationRequestUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns((Claim?)null);

            // Act
            var result = await _deletePublicationRequestUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenPublicationRequestNotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync((PublicationRequest?)null);

            // Act
            var result = await _deletePublicationRequestUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoPublicationRequestFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var PublicationRequest = new PublicationRequest
            {
                Id = Guid.NewGuid(),
                Status = PublicationRequestStatus.Pending,
                Knowledge = new Knowledge
                {
                    Title = "",
                    Id = guid,
                    CreatorId = Guid.NewGuid(),
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });

            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(PublicationRequest);

            // Act
            var result = await _deletePublicationRequestUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenPublicationRequestAlreadyApproved()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var PublicationRequest = new PublicationRequest
            {
                Id = Guid.NewGuid(),
                Status = PublicationRequestStatus.Approved,
                Knowledge = new Knowledge
                {
                    Title = "",
                    Id = guid,
                    CreatorId = userId,
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(PublicationRequest);

            // Act
            var result = await _deletePublicationRequestUseCase.Execute(guid);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.PublicationRequestAlreadyApproved, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestIsDeleted()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var PublicationRequest = new PublicationRequest
            {
                Id = Guid.NewGuid(),
                Status = PublicationRequestStatus.Pending,
                Knowledge = new Knowledge
                {
                    Title = "",
                    Id = guid,
                    CreatorId = userId,
                }
            };

            _httpContextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, UserName = "Test", Email = "" });
            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(PublicationRequest);
            _publicationRequestRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(PublicationRequest);

            // Act
            var result = await _deletePublicationRequestUseCase.Execute(guid);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(PublicationRequest.Id, result.Value.Id);
        }
    }
}