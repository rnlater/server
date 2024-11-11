using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.UseCases.Knowledges.KnowledgeTopics;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;

namespace UnitTests.Knowledges.KnowledgeTopics
{
    public class UpdateKnowledgeTopicUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateKnowledgeTopicUseCase _updateKnowledgeTopicUseCase;

        public UpdateKnowledgeTopicUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);

            _updateKnowledgeTopicUseCase = new UpdateKnowledgeTopicUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicNotFound()
        {
            var parameters = new UpdateKnowledgeTopicParams
            {
                Id = Guid.NewGuid(),
                Title = "Updated KnowledgeTopic",
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            var result = await _updateKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenParentCannotBeItself()
        {
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var parameters = new UpdateKnowledgeTopicParams
            {
                Id = knowledgeTopic.Id,
                Title = knowledgeTopic.Title,
                ParentId = knowledgeTopic.Id,
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);

            var result = await _updateKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.CannotBeParentOfItself, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenParentNotFound()
        {
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var parameters = new UpdateKnowledgeTopicParams
            {
                Id = knowledgeTopic.Id,
                Title = knowledgeTopic.Title,
                ParentId = Guid.NewGuid(),
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);

            var result = await _updateKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }


        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsUpdated()
        {
            var knowledgeTopic = SeedData.GetKnowledgeTopics()[0];
            var parameters = new UpdateKnowledgeTopicParams
            {
                Id = knowledgeTopic.Id,
                Title = "Updated KnowledgeTopic",
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopic);
            _knowledgeTopicRepositoryMock.Setup(r => r.Update(It.IsAny<KnowledgeTopic>())).ReturnsAsync(knowledgeTopic);

            var result = await _updateKnowledgeTopicUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Title, result.Value.Title);
        }
    }
}
