using Application.UseCases.Knowledges.KnowledgeTopics;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.KnowledgeTopics
{
    public class AttachDetachKnowledgesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopicKnowledge>> _knowledgeTopicKnowledgeRepositoryMock;
        private readonly AttachDetachKnowledgesUseCase _attachDetachKnowledgesUseCase;

        public AttachDetachKnowledgesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTopicKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTopicKnowledge>>();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopicKnowledge>()).Returns(_knowledgeTopicKnowledgeRepositoryMock.Object);

            _attachDetachKnowledgesUseCase = new AttachDetachKnowledgesUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicNotFound()
        {
            // Arrange
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTopicId = Guid.NewGuid(), KnowledgeIds = new List<Guid> { Guid.NewGuid() } };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTopicId = knowledgeTopic.Id, KnowledgeIds = new List<Guid> { Guid.NewGuid() } };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicKnowledgesAreAttached()
        {
            // Arrange
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTopicId = knowledgeTopic.Id, KnowledgeIds = new List<Guid> { knowledge.Id } };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopicKnowledge>>())).ReturnsAsync((KnowledgeTopicKnowledge?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _knowledgeTopicKnowledgeRepositoryMock.Verify(r => r.Add(It.IsAny<KnowledgeTopicKnowledge>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicKnowledgesAreDetached()
        {
            // Arrange
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var knowledgeTopicKnowledge = new KnowledgeTopicKnowledge { KnowledgeTopicId = knowledgeTopic.Id, KnowledgeId = knowledge.Id };
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTopicId = knowledgeTopic.Id, KnowledgeIds = new List<Guid> { knowledge.Id } };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopicKnowledge>>())).ReturnsAsync(knowledgeTopicKnowledge);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _knowledgeTopicKnowledgeRepositoryMock.Verify(r => r.Delete(It.IsAny<KnowledgeTopicKnowledge>()), Times.Once);
        }
    }
}