using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;

namespace UnitTests.Knowledges
{
    public class GetDetailedKnowledgeByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetDetailedKnowledgeByGuidUseCase _getDetailedKnowledgeByGuidUseCase;

        public GetDetailedKnowledgeByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);

            _getDetailedKnowledgeByGuidUseCase = new GetDetailedKnowledgeByGuidUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            var knowledgeId = Guid.NewGuid();

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            var result = await _getDetailedKnowledgeByGuidUseCase.Execute(knowledgeId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsFound()
        {
            var knowledge = new Knowledge
            {
                Id = Guid.NewGuid(),
                Title = "Test Knowledge",
                KnowledgeTypeKnowledges = [],
                KnowledgeTopicKnowledges = [],
                SubjectKnowledges = [],
                Creator = SeedData.GetUsers().First(),
                Materials = []
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);

            var result = await _getDetailedKnowledgeByGuidUseCase.Execute(knowledge.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledge.Id, result.Value.Id);
            Assert.Equal(knowledge.Title, result.Value.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WithRelatedEntities()
        {
            var knowledge = new Knowledge
            {
                Id = Guid.NewGuid(),
                Title = "Test Knowledge",
                KnowledgeTypeKnowledges = SeedData.GetKnowledgeTypeKnowledges(),
                KnowledgeTopicKnowledges = SeedData.GetKnowledgeTopicKnowledges(),
                SubjectKnowledges = SeedData.GetSubjectKnowledges(),
                Creator = SeedData.GetUsers().First(),
                Materials = SeedData.GetMaterials()
            };

            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);

            var result = await _getDetailedKnowledgeByGuidUseCase.Execute(knowledge.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledge.Id, result.Value.Id);
            Assert.Equal(knowledge.Title, result.Value.Title);
            Assert.NotEmpty(result.Value.KnowledgeTypeKnowledges);
            Assert.NotEmpty(result.Value.KnowledgeTopicKnowledges);
            Assert.NotEmpty(result.Value.SubjectKnowledges);
            Assert.NotNull(result.Value.Creator);
            Assert.NotEmpty(result.Value.Materials);
        }
    }
}
