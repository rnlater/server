using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.UseCases.Knowledges.KnowledgeTypes;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class UpdateKnowledgeTypeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateKnowledgeTypeUseCase _updateKnowledgeTypeUseCase;

        public UpdateKnowledgeTypeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);

            _updateKnowledgeTypeUseCase = new UpdateKnowledgeTypeUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeNotFound()
        {
            var parameters = new UpdateKnowledgeTypeParams
            {
                Id = Guid.NewGuid(),
                Name = "Updated KnowledgeType",
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            var result = await _updateKnowledgeTypeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenParentCannotBeItself()
        {
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var parameters = new UpdateKnowledgeTypeParams
            {
                Id = knowledgeType.Id,
                Name = knowledgeType.Name,
                ParentId = knowledgeType.Id,
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);

            var result = await _updateKnowledgeTypeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.CannotBeParentOfItself, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenParentNotFound()
        {
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var parameters = new UpdateKnowledgeTypeParams
            {
                Id = knowledgeType.Id,
                Name = knowledgeType.Name,
                ParentId = Guid.NewGuid(),
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            var result = await _updateKnowledgeTypeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }


        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsUpdated()
        {
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];
            var parameters = new UpdateKnowledgeTypeParams
            {
                Id = knowledgeType.Id,
                Name = "Updated KnowledgeType",
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);
            _knowledgeTypeRepositoryMock.Setup(r => r.Update(It.IsAny<KnowledgeType>())).ReturnsAsync(knowledgeType);

            var result = await _updateKnowledgeTypeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Name, result.Value.Name);
        }
    }
}
