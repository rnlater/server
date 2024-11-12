using Application.UseCases.Knowledges;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class AttachDeattachKnowledgeTypeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTypeKnowledge>> _knowledgeTypeKnowledgeRepositoryMock;
        private readonly AttachDeattachKnowledgeTypeUseCase _attachDeattachKnowledgeTypeUseCase;

        public AttachDeattachKnowledgeTypeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTypeKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTypeKnowledge>>();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTypeKnowledge>()).Returns(_knowledgeTypeKnowledgeRepositoryMock.Object);

            _attachDeattachKnowledgeTypeUseCase = new AttachDeattachKnowledgeTypeUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeNotFound()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTypeParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTypeId = Guid.NewGuid()
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            // Act
            var result = await _attachDeattachKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTypeParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTypeId = Guid.NewGuid()
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(new KnowledgeType { Id = parameters.KnowledgeTypeId, Name = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _attachDeattachKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsAttached()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTypeParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTypeId = Guid.NewGuid()
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(new KnowledgeType { Id = parameters.KnowledgeTypeId, Name = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeId, Title = "" });
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTypeKnowledge>>())).ReturnsAsync((KnowledgeTypeKnowledge?)null);

            // Act
            var result = await _attachDeattachKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Created, result.Value);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsDetached()
        {
            // Arrange
            var parameters = new AttachDeattachKnowledgeTypeParams
            {
                KnowledgeId = Guid.NewGuid(),
                KnowledgeTypeId = Guid.NewGuid()
            };

            var knowledgeTypeKnowledge = new KnowledgeTypeKnowledge
            {
                KnowledgeId = parameters.KnowledgeId,
                KnowledgeTypeId = parameters.KnowledgeTypeId
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(new KnowledgeType { Id = parameters.KnowledgeTypeId, Name = "" });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeId, Title = "" });
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTypeKnowledge>>())).ReturnsAsync(knowledgeTypeKnowledge);

            // Act
            var result = await _attachDeattachKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Deleted, result.Value);
        }
    }
}