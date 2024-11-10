using Application.UseCases.Knowledges.KnowledgeTypes;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class AttachDetachKnowledgesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTypeKnowledge>> _knowledgeTypeKnowledgeRepositoryMock;
        private readonly AttachDetachKnowledgesUseCase _attachDetachKnowledgesUseCase;

        public AttachDetachKnowledgesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTypeKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTypeKnowledge>>();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTypeKnowledge>()).Returns(_knowledgeTypeKnowledgeRepositoryMock.Object);

            _attachDetachKnowledgesUseCase = new AttachDetachKnowledgesUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeNotFound()
        {
            // Arrange
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTypeId = Guid.NewGuid(), KnowledgeIds = new List<Guid> { Guid.NewGuid() } };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTypeId = knowledgeType.Id, KnowledgeIds = new List<Guid> { Guid.NewGuid() } };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeKnowledgesAreAttached()
        {
            // Arrange
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTypeId = knowledgeType.Id, KnowledgeIds = new List<Guid> { knowledge.Id } };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTypeKnowledge>>())).ReturnsAsync((KnowledgeTypeKnowledge?)null);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _knowledgeTypeKnowledgeRepositoryMock.Verify(r => r.Add(It.IsAny<KnowledgeTypeKnowledge>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeKnowledgesAreDetached()
        {
            // Arrange
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var knowledgeTypeKnowledge = new KnowledgeTypeKnowledge { KnowledgeTypeId = knowledgeType.Id, KnowledgeId = knowledge.Id };
            var parameters = new AttachDetachKnowledgesParams { KnowledgeTypeId = knowledgeType.Id, KnowledgeIds = new List<Guid> { knowledge.Id } };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTypeKnowledge>>())).ReturnsAsync(knowledgeTypeKnowledge);

            // Act
            var result = await _attachDetachKnowledgesUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
            _knowledgeTypeKnowledgeRepositoryMock.Verify(r => r.Delete(It.IsAny<KnowledgeTypeKnowledge>()), Times.Once);
        }
    }
}