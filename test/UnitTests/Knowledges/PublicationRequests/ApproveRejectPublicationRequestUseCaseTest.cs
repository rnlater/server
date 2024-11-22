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
    public class ApproveRejectPublicationRequestUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<PublicationRequest>> _publicationRequestRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly ApproveRejectPublicationRequestUseCase _approveRejectPublicationRequestUseCase;

        public ApproveRejectPublicationRequestUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _publicationRequestRepositoryMock = new Mock<IRepository<PublicationRequest>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<PublicationRequest>()).Returns(_publicationRequestRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);

            _approveRejectPublicationRequestUseCase = new ApproveRejectPublicationRequestUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenPublicationRequestNotFound()
        {
            // Arrange
            var parameters = new ApproveRejectPublicationRequestParams
            {
                PublicationRequestId = Guid.NewGuid(),
                IsApproved = true
            };

            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync((PublicationRequest?)null);

            // Act
            var result = await _approveRejectPublicationRequestUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoPublicationRequestFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenPublicationRequestAlreadyApprovedOrRejected()
        {
            // Arrange
            var parameters = new ApproveRejectPublicationRequestParams
            {
                PublicationRequestId = Guid.NewGuid(),
                IsApproved = true
            };

            var publicationRequest = new PublicationRequest
            {
                Id = parameters.PublicationRequestId,
                Status = PublicationRequestStatus.Approved
            };

            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(publicationRequest);

            // Act
            var result = await _approveRejectPublicationRequestUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.PublicationRequestAlreadyApprovedOrRejected, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestIsApproved()
        {
            // Arrange
            var parameters = new ApproveRejectPublicationRequestParams
            {
                PublicationRequestId = Guid.NewGuid(),
                IsApproved = true
            };

            var publicationRequest = new PublicationRequest
            {
                Id = parameters.PublicationRequestId,
                Status = PublicationRequestStatus.Pending,
                Knowledge = new Knowledge
                {
                    Title = "",
                    Id = Guid.NewGuid(),
                    Visibility = KnowledgeVisibility.Private
                }
            };

            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(publicationRequest);
            _knowledgeRepositoryMock.Setup(r => r.Update(It.IsAny<Knowledge>())).ReturnsAsync(publicationRequest.Knowledge);
            _publicationRequestRepositoryMock.Setup(r => r.Update(It.IsAny<PublicationRequest>())).ReturnsAsync(publicationRequest);

            // Act
            var result = await _approveRejectPublicationRequestUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(publicationRequest.Id, result.Value.Id);
            Assert.Equal(PublicationRequestStatus.Approved.ToString(), result.Value.Status);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestIsRejected()
        {
            // Arrange
            var parameters = new ApproveRejectPublicationRequestParams
            {
                PublicationRequestId = Guid.NewGuid(),
                IsApproved = false
            };

            var publicationRequest = new PublicationRequest
            {
                Id = parameters.PublicationRequestId,
                Status = PublicationRequestStatus.Pending,
                Knowledge = new Knowledge
                {
                    Id = Guid.NewGuid(),
                    Title = "",
                    Visibility = KnowledgeVisibility.Private
                }
            };

            _publicationRequestRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(publicationRequest);
            _publicationRequestRepositoryMock.Setup(r => r.Update(It.IsAny<PublicationRequest>())).ReturnsAsync(publicationRequest);

            // Act
            var result = await _approveRejectPublicationRequestUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(publicationRequest.Id, result.Value.Id);
            Assert.Equal(PublicationRequestStatus.Rejected.ToString(), result.Value.Status);
        }
    }
}