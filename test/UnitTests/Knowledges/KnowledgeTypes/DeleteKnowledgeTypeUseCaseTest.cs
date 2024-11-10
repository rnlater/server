using Application.Mappings;
using Application.UseCases.Knowledges.KnowledgeTypes;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class DeleteKnowledgeTypeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteKnowledgeTypeUseCase _deleteKnowledgeTypeUseCase;

        public DeleteKnowledgeTypeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);

            _deleteKnowledgeTypeUseCase = new DeleteKnowledgeTypeUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeNotFound()
        {
            // Arrange
            var knowledgeTypeId = Guid.NewGuid();

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            // Act
            var result = await _deleteKnowledgeTypeUseCase.Execute(knowledgeTypeId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsDeleted()
        {
            // Arrange
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeTypeRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(knowledgeType);

            // Act
            var result = await _deleteKnowledgeTypeUseCase.Execute(knowledgeType.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeType.Id, result.Value.Id);
            Assert.Equal(knowledgeType.Name, result.Value.Name);
        }
    }
}
