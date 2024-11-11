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
    public class DeleteKnowledgeTopicUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteKnowledgeTopicUseCase _deleteKnowledgeTopicUseCase;

        public DeleteKnowledgeTopicUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);

            _deleteKnowledgeTopicUseCase = new DeleteKnowledgeTopicUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicNotFound()
        {
            var knowledgeTopicId = Guid.NewGuid();

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            var result = await _deleteKnowledgeTopicUseCase.Execute(knowledgeTopicId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsDeleted()
        {
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeTopicRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(knowledgeTopic);

            var result = await _deleteKnowledgeTopicUseCase.Execute(knowledgeTopic.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(knowledgeTopic.Id, result.Value.Id);
            Assert.Equal(knowledgeTopic.Title, result.Value.Title);
        }
    }
}
