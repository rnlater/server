using Application.UseCases.Knowledges.KnowledgeTypes;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.Mappings;
using Domain.Base;
using Domain.Entities.PivotEntities;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class CreateKnowledgeTypeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTypeKnowledge>> _knowledgeTypeKnowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateKnowledgeTypeUseCase _createKnowledgeTypeUseCase;

        public CreateKnowledgeTypeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTypeKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTypeKnowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTypeKnowledge>()).Returns(_knowledgeTypeKnowledgeRepositoryMock.Object);

            _createKnowledgeTypeUseCase = new CreateKnowledgeTypeUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeAlreadyExists()
        {
            // Arrange
            var parameters = new CreateKnowledgeTypeParams
            {
                Name = "Existing KnowledgeType",
                ParentId = null,
                KnowledgeIds = new List<Guid> { Guid.NewGuid() }
            };

            var existingKnowledgeType = new KnowledgeType { Id = Guid.NewGuid(), Name = parameters.Name };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(existingKnowledgeType);

            // Act
            var result = await _createKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.KnowledgeTypeAlreadyExists, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsCreated()
        {
            // Arrange
            var parameters = new CreateKnowledgeTypeParams
            {
                Name = "New KnowledgeType",
                ParentId = null,
                KnowledgeIds = new List<Guid> { Guid.NewGuid() }
            };

            var newKnowledgeType = new KnowledgeType { Id = Guid.NewGuid(), Name = parameters.Name };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);
            _knowledgeTypeRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeType>())).ReturnsAsync(newKnowledgeType);

            _knowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeIds[0], Title = "Knowledge" });
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeTypeKnowledge>())).ReturnsAsync(new KnowledgeTypeKnowledge());

            // Act
            var result = await _createKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Name, result.Value.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenParentKnowledgeTypeNotFoundWithGuid()
        {
            // Arrange
            var ParentId = Guid.NewGuid();
            var parameters = new CreateKnowledgeTypeParams
            {
                Name = "New KnowledgeType",
                ParentId = ParentId,
                KnowledgeIds = new List<Guid> { Guid.NewGuid() }
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.GetById(ParentId)).ReturnsAsync((KnowledgeType?)null);

            // Act
            var result = await _createKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeFoundWithGuid()
        {
            // Arrange
            var parameters = new CreateKnowledgeTypeParams
            {
                Name = "New KnowledgeType",
                ParentId = null,
                KnowledgeIds = new List<Guid> { Guid.NewGuid() }
            };

            _knowledgeRepositoryMock.Setup(r => r.GetById(parameters.KnowledgeIds[0])).ReturnsAsync((Knowledge?)null);

            // Act
            var result = await _createKnowledgeTypeUseCase.Execute(parameters);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }
    }
}
