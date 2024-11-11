using Application.UseCases.Knowledges.KnowledgeTopics;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.Mappings;
using Domain.Base;
using Domain.Entities.PivotEntities;

namespace UnitTests.Knowledges.KnowledgeTopics
{
    public class CreateKnowledgeTopicUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopicKnowledge>> _knowledgeTopicKnowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly CreateKnowledgeTopicUseCase _createKnowledgeTopicUseCase;

        public CreateKnowledgeTopicUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTopicKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTopicKnowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopicKnowledge>()).Returns(_knowledgeTopicKnowledgeRepositoryMock.Object);

            _createKnowledgeTopicUseCase = new CreateKnowledgeTopicUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeTopicAlreadyExists()
        {
            var parameters = new CreateKnowledgeTopicParams
            {
                Title = "Existing KnowledgeTopic",
                ParentId = null,
                KnowledgeIds = [Guid.NewGuid()]
            };

            var existingKnowledgeTopic = new KnowledgeTopic { Id = Guid.NewGuid(), Title = parameters.Title };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(existingKnowledgeTopic);

            var result = await _createKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.KnowledgeTopicAlreadyExists, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicIsCreated()
        {
            var parameters = new CreateKnowledgeTopicParams
            {
                Title = "New KnowledgeTopic",
                ParentId = null,
                KnowledgeIds = [Guid.NewGuid()]
            };

            var newKnowledgeTopic = new KnowledgeTopic { Id = Guid.NewGuid(), Title = parameters.Title };

            _knowledgeTopicRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync((KnowledgeTopic?)null);
            _knowledgeTopicRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeTopic>())).ReturnsAsync(newKnowledgeTopic);

            _knowledgeRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(new Knowledge { Id = parameters.KnowledgeIds[0], Title = "Knowledge" });
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeTopicKnowledge>())).ReturnsAsync(new KnowledgeTopicKnowledge());

            var result = await _createKnowledgeTopicUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Title, result.Value.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenParentKnowledgeTopicNotFoundWithGuid()
        {
            var ParentId = Guid.NewGuid();
            var parameters = new CreateKnowledgeTopicParams
            {
                Title = "New KnowledgeTopic",
                ParentId = ParentId,
                KnowledgeIds = [Guid.NewGuid()]
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.GetById(ParentId)).ReturnsAsync((KnowledgeTopic?)null);

            var result = await _createKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeFoundWithGuid()
        {
            var parameters = new CreateKnowledgeTopicParams
            {
                Title = "New KnowledgeTopic",
                ParentId = null,
                KnowledgeIds = [Guid.NewGuid()]
            };

            _knowledgeRepositoryMock.Setup(r => r.GetById(parameters.KnowledgeIds[0])).ReturnsAsync((Knowledge?)null);

            var result = await _createKnowledgeTopicUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }
    }
}
