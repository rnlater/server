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
    public class UpdateKnowledgeVisibilityUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<PublicationRequest>> _publicationRequestRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateKnowledgeVisibilityUseCase _updateKnowledgeVisibilityUseCase;

        public UpdateKnowledgeVisibilityUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _publicationRequestRepositoryMock = new Mock<IRepository<PublicationRequest>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<PublicationRequest>()).Returns(_publicationRequestRepositoryMock.Object);

            _updateKnowledgeVisibilityUseCase = new UpdateKnowledgeVisibilityUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var parameters = new UpdateKnowledgeVisibilityParams
            {
                KnowledgeId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Public
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _updateKnowledgeVisibilityUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoChangeDetected()
        {
            // Arrange
            var parameters = new UpdateKnowledgeVisibilityParams
            {
                KnowledgeId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Public
            };

            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                Visibility = KnowledgeVisibility.Public
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);

            // Act
            var result = await _updateKnowledgeVisibilityUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoChangeDetected, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeVisibilityIsUpdated()
        {
            // Arrange
            var parameters = new UpdateKnowledgeVisibilityParams
            {
                KnowledgeId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Public
            };

            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                Visibility = KnowledgeVisibility.Private
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeRepositoryMock.Setup(r => r.Update(It.IsAny<Knowledge>())).ReturnsAsync(knowledge);

            // Act
            var result = await _updateKnowledgeVisibilityUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledge.Id, result.Value.Id);
            Assert.Equal(parameters.Visibility.ToString(), result.Value.Visibility);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenPublicationRequestIsUpdated()
        {
            // Arrange
            var parameters = new UpdateKnowledgeVisibilityParams
            {
                KnowledgeId = Guid.NewGuid(),
                Visibility = KnowledgeVisibility.Public
            };

            var knowledge = new Knowledge
            {
                Title = "",
                Id = parameters.KnowledgeId,
                Visibility = KnowledgeVisibility.Private,
                PublicationRequest = new PublicationRequest
                {
                    Id = Guid.NewGuid(),
                    Status = PublicationRequestStatus.Pending
                }
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeRepositoryMock.Setup(r => r.Update(It.IsAny<Knowledge>())).ReturnsAsync(knowledge);
            _publicationRequestRepositoryMock.Setup(r => r.Update(It.IsAny<PublicationRequest>())).ReturnsAsync(knowledge.PublicationRequest);

            // Act
            var result = await _updateKnowledgeVisibilityUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledge.Id, result.Value.Id);
            Assert.Equal(parameters.Visibility.ToString(), result.Value.Visibility);
            Assert.Equal(PublicationRequestStatus.Approved, knowledge.PublicationRequest.Status);
        }
    }
}