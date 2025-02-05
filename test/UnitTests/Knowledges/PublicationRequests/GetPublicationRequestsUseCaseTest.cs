using Application.Mappings;
using Application.UseCases.Knowledges.PublicationRequests;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.PublicationRequests
{
    public class GetPublicationRequestsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<PublicationRequest>> _publicationRequestRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetPublicationRequestsUseCase _getPublicationRequestsUseCase;

        public GetPublicationRequestsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _publicationRequestRepositoryMock = new Mock<IRepository<PublicationRequest>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<PublicationRequest>()).Returns(_publicationRequestRepositoryMock.Object);

            _getPublicationRequestsUseCase = new GetPublicationRequestsUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoPublicationRequestsFound()
        {
            // Arrange
            var parameters = new GetPublicationRequestsParams
            {
                SearchTerm = "Test",
                Page = 1,
                PageSize = 10,
                Status = PublicationRequestStatus.Pending
            };

            _publicationRequestRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(Enumerable.Empty<PublicationRequest>());

            // Act
            var result = await _getPublicationRequestsUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoPublicationRequestsFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestsAreFound()
        {
            // Arrange
            var parameters = new GetPublicationRequestsParams
            {
                SearchTerm = "Test",
                Page = 1,
                PageSize = 10,
                Status = PublicationRequestStatus.Pending
            };

            var publicationRequests = new List<PublicationRequest>
            {
                new PublicationRequest
                {
                    Id = Guid.NewGuid(),
                    Knowledge = new Knowledge
                    {
                        Id = Guid.NewGuid(),
                        Title = "Test Knowledge",
                        Creator = new User
                        {
                            Id = Guid.NewGuid(),
                            UserName = "Test User",
                            Email = ""
                        }
                    },
                    Status = PublicationRequestStatus.Pending
                }
            };

            _publicationRequestRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<PublicationRequest>>())).ReturnsAsync(publicationRequests);

            // Act
            var result = await _getPublicationRequestsUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(publicationRequests.Count, result.Value.Count());
        }
    }
}