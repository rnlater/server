using Application.Mappings;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class UpdateKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateKnowledgeUseCase _updateKnowledgeUseCase;

        public UpdateKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);

            _updateKnowledgeUseCase = new UpdateKnowledgeUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            // Arrange
            var parameters = new UpdateKnowledgeParams
            {
                Id = Guid.NewGuid(),
                Title = "Updated Knowledge",
                Level = KnowledgeLevel.Intermediate
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _updateKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsUpdated()
        {
            // Arrange
            var knowledgeId = Guid.NewGuid();
            var knowledge = new Knowledge { Id = knowledgeId, Title = "Old Knowledge", Level = KnowledgeLevel.Beginner };

            var parameters = new UpdateKnowledgeParams
            {
                Id = knowledgeId,
                Title = "Updated Knowledge",
                Level = KnowledgeLevel.Intermediate
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _knowledgeRepositoryMock.Setup(r => r.Update(It.IsAny<Knowledge>())).ReturnsAsync(knowledge);

            // Act
            var result = await _updateKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeId, result.Value.Id);
            Assert.Equal(parameters.Title, result.Value.Title);
            Assert.Equal(parameters.Level.ToString(), result.Value.Level);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            // Arrange
            var parameters = new UpdateKnowledgeParams
            {
                Id = Guid.NewGuid(),
                Title = "Updated Knowledge",
                Level = KnowledgeLevel.Intermediate
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ThrowsAsync(new Exception());

            // Act
            var result = await _updateKnowledgeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError, result.Error);
        }
    }
}