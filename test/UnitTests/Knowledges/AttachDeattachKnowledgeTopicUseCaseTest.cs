using Application.UseCases.Knowledges;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class AttachDeattachKnowledgeTopicUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopicKnowledge>> _knowledgeTopicKnowledgeRepositoryMock;
        private readonly AttachDeattachKnowledgeTopicUseCase _attachDeattachKnowledgeTopicUseCase;

        public AttachDeattachKnowledgeTopicUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTopicKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTopicKnowledge>>();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopicKnowledge>()).Returns(_knowledgeTopicKnowledgeRepositoryMock.Object);

            _attachDeattachKnowledgeTopicUseCase = new AttachDeattachKnowledgeTopicUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicNotFound()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTopicParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTopicId = Guid.NewGuid()
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            // Act
            var result = await _attachDeattachKnowledgeTopicUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTopicParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTopicId = Guid.NewGuid()
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(new KnowledgeTopic { Id = parameters.KnowledgeTopicId, Title = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _attachDeattachKnowledgeTopicUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsAttached()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTopicParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTopicId = Guid.NewGuid()
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(new KnowledgeTopic { Id = parameters.KnowledgeTopicId, Title = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeId, Title = "" });
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopicKnowledge>>())).ReturnsAsync((KnowledgeTopicKnowledge?)null);

            // Act
            var result = await _attachDeattachKnowledgeTopicUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Created, result.Value);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsDetached()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTopicParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTopicId = Guid.NewGuid()
            };

            var knowledgeTopicKnowledge = new KnowledgeTopicKnowledge
            {
                KnowledgeId = parameters.KnowledgeId,
                KnowledgeTopicId = parameters.KnowledgeTopicId
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(new KnowledgeTopic { Id = parameters.KnowledgeTopicId, Title = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeId, Title = "" });
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopicKnowledge>>())).ReturnsAsync(knowledgeTopicKnowledge);

            // Act
            var result = await _attachDeattachKnowledgeTopicUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Deleted, result.Value);
        }
    }
}