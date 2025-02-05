using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.UseCases.Knowledges.KnowledgeTopics;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;
using Application.Interfaces;
using Application.DTOs;

namespace UnitTests.Knowledges.KnowledgeTopics
{
    public class GetKnowledgeTopicsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgeTopicsUseCase _getKnowledgeTopicsUseCase;

        public GetKnowledgeTopicsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);

            _getKnowledgeTopicsUseCase = new GetKnowledgeTopicsUseCase(_unitOfWorkMock.Object, _mapper, _cacheMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTopicsFound()
        {
            _cacheMock.Setup(c => c.GetAsync<IEnumerable<KnowledgeTopicDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<KnowledgeTopicDto>?)null);
            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(Enumerable.Empty<KnowledgeTopic>());
            var Params = new GetKnowledgeTopicsParams { };

            var result = await _getKnowledgeTopicsUseCase.Execute(Params);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicsFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTopicsAreFound()
        {
            var knowledgeTopics = SeedData.GetKnowledgeTopics();
            var knowledgeTopicDtos = knowledgeTopics.Select(kt => new KnowledgeTopicDto { Id = kt.Id, Title = kt.Title });

            _cacheMock.Setup(c => c.GetAsync<IEnumerable<KnowledgeTopicDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<KnowledgeTopicDto>?)null);
            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopics);
            _cacheMock.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KnowledgeTopicDto>>(), null)).Returns(Task.CompletedTask);
            var Params = new GetKnowledgeTopicsParams { };

            var result = await _getKnowledgeTopicsUseCase.Execute(Params);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledgeTopics.Length, result.Value.Count());

            var knowledgeTopicDto = result.Value.First();
            Assert.Equal(knowledgeTopics[0].Id, knowledgeTopicDto.Id);
            Assert.Equal(knowledgeTopics[0].Title, knowledgeTopicDto.Title);
        }
    }
}
