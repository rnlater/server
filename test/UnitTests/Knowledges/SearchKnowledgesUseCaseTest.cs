using Application.DTOs;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;
using Application.Mappings;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Domain.Entities.PivotEntities;

namespace UnitTests.Knowledges
{
    public class SearchKnowledgesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly IMapper _mapper;
        private readonly SearchKnowledgesUseCase _searchKnowledgesUseCase;

        public SearchKnowledgesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);

            _searchKnowledgesUseCase = new SearchKnowledgesUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgesFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = "NonExistentKnowledge",
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(Enumerable.Empty<Knowledge>());

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgesAreFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = "Introduction",
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledges = SeedData.GetKnowledges();

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Length, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenFilteredByKnowledgeType()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid> { SeedData.KnowledgeType1Id },
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledgeTypes = SeedData.GetKnowledgeTypes();
            knowledgeTypes[1].Children = new List<KnowledgeType> { knowledgeTypes[0] };

            var knowledgeTypeKnowledge = SeedData.GetKnowledgeTypeKnowledges()[1];
            knowledgeTypeKnowledge.KnowledgeType = knowledgeTypes[1];

            var knowledges = new List<Knowledge> { SeedData.GetKnowledges()[0] };
            knowledges[0].KnowledgeTypeKnowledges = new List<KnowledgeTypeKnowledge> { knowledgeTypeKnowledge };

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync([knowledgeTypes[1]]);
            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Count, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenFilteredByKnowledgeTopic()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid> { SeedData.KnowledgeTopic1Id },
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledgeTopics = SeedData.GetKnowledgeTopics();
            knowledgeTopics[1].Children = new List<KnowledgeTopic> { knowledgeTopics[0] };

            var knowledgeTopicKnowledge = SeedData.GetKnowledgeTopicKnowledges()[1];
            knowledgeTopicKnowledge.KnowledgeTopic = knowledgeTopics[1];

            var knowledges = new List<Knowledge> { SeedData.GetKnowledges()[0] };
            knowledges[0].KnowledgeTopicKnowledges = new List<KnowledgeTopicKnowledge> { knowledgeTopicKnowledge };

            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync([knowledgeTopics[1]]);
            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Count, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenFilteredByLevel()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid>(),
                Level = KnowledgeLevel.Beginner,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledges = SeedData.GetKnowledges().Where(k => k.Level == KnowledgeLevel.Beginner).ToList();

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Count, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenOrderedByTitle()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Title,
                Ascending = true
            };

            var knowledges = SeedData.GetKnowledges().OrderBy(k => k.Title).ToList();

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Count, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTypesFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid> { Guid.NewGuid() },
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(Enumerable.Empty<KnowledgeType>());

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgeTypesNotFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                KnowledgeTopicIds = new List<Guid>(),
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledgeTypes = new List<KnowledgeType>
            {
                new KnowledgeType { Id = parameters.KnowledgeTypeIds[0], Name = "Type 1" }
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeTypes);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgeTypesNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTopicsFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid> { Guid.NewGuid() },
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(Enumerable.Empty<KnowledgeTopic>());

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicsFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSomeKnowledgeTopicsNotFound()
        {
            var parameters = new SearchKnowledgesParameters
            {
                SearchTerm = null,
                Page = 1,
                PageSize = 10,
                KnowledgeTypeIds = new List<Guid>(),
                KnowledgeTopicIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                Level = null,
                OrderBy = SearchKnowledgesParameters.OrderByType.Date,
                Ascending = false
            };

            var knowledgeTopics = new List<KnowledgeTopic>
            {
                new KnowledgeTopic { Id = parameters.KnowledgeTopicIds[0], Title = "Topic 1" }
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopics);

            var result = await _searchKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.SomeKnowledgeTopicsNotFound, result.Error);
        }
    }
}