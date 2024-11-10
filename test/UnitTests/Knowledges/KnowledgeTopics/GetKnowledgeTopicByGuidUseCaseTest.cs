using Application.Mappings;
using Application.UseCases.Knowledges.KnowledgeTopics;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges.KnowledgeTopics
{
    public class GetKnowledgeTopicByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgeTopicByGuidUseCase _getKnowledgeTopicByGuidUseCase;

        public GetKnowledgeTopicByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);

            _getKnowledgeTopicByGuidUseCase = new GetKnowledgeTopicByGuidUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicNotFound()
        {
            // Arrange
            var knowledgeTopicId = Guid.NewGuid();

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            // Act
            var result = await _getKnowledgeTopicByGuidUseCase.Execute(knowledgeTopicId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsFound()
        {
            // Arrange
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);

            // Act
            var result = await _getKnowledgeTopicByGuidUseCase.Execute(knowledgeTopic.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeTopic.Id, result.Value.Id);
            Assert.Equal(knowledgeTopic.Title, result.Value.Title);
        }
    }
}
