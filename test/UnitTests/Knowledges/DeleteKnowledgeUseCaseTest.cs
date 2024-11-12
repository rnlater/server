using Application.Mappings;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class DeleteKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteKnowledgeUseCase _deleteKnowledgeUseCase;

        public DeleteKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);

            _deleteKnowledgeUseCase = new DeleteKnowledgeUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var knowledgeId = Guid.NewGuid();

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _deleteKnowledgeUseCase.Execute(knowledgeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsDeleted()
        {
            // Arrange
            var knowledgeId = Guid.NewGuid();
            var knowledge = new Knowledge { Id = knowledgeId, Title = "Test Knowledge" };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(knowledge);

            // Act
            var result = await _deleteKnowledgeUseCase.Execute(knowledgeId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeId, result.Value.Id);
            Assert.Equal(knowledge.Title, result.Value.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            // Arrange
            var knowledgeId = Guid.NewGuid();

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ThrowsAsync(new Exception());

            // Act
            var result = await _deleteKnowledgeUseCase.Execute(knowledgeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError, result.Error);
        }
    }
}