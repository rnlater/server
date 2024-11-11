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
    public class GetKnowledgeTypeByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgeTypeByGuidUseCase _getKnowledgeTypeByGuidUseCase;

        public GetKnowledgeTypeByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);

            _getKnowledgeTypeByGuidUseCase = new GetKnowledgeTypeByGuidUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTypeNotFound()
        {
            var knowledgeTypeId = Guid.NewGuid();

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync((KnowledgeType?)null);

            var result = await _getKnowledgeTypeByGuidUseCase.Execute(knowledgeTypeId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypeIsFound()
        {
            var knowledgeType = SeedData.GetKnowledgeTypes()[0];

            _knowledgeTypeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeType);

            var result = await _getKnowledgeTypeByGuidUseCase.Execute(knowledgeType.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeType.Id, result.Value.Id);
            Assert.Equal(knowledgeType.Name, result.Value.Name);
        }
    }
}
