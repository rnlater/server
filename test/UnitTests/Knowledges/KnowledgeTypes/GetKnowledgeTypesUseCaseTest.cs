using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Application.UseCases.Knowledges.KnowledgeTypes;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;
using Application.Interfaces;
using Application.DTOs;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class GetKnowledgeTypesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgeTypesUseCase _getKnowledgeTypesUseCase;

        public GetKnowledgeTypesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);

            _getKnowledgeTypesUseCase = new GetKnowledgeTypesUseCase(_unitOfWorkMock.Object, _mapper, _cacheMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTypesFound()
        {
            _cacheMock.Setup(c => c.GetAsync<IEnumerable<KnowledgeTypeDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<KnowledgeTypeDto>?)null);
            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(Enumerable.Empty<KnowledgeType>());

            var Params = new GetKnowledgeTypesParams { };
            var result = await _getKnowledgeTypesUseCase.Execute(Params);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypesAreFound()
        {
            var knowledgeTypes = SeedData.GetKnowledgeTypes();
            var knowledgeTypeDtos = _mapper.Map<IEnumerable<KnowledgeTypeDto>>(knowledgeTypes);

            _cacheMock.Setup(c => c.GetAsync<IEnumerable<KnowledgeTypeDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<KnowledgeTypeDto>?)null);
            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeTypes);

            var Params = new GetKnowledgeTypesParams { };
            var result = await _getKnowledgeTypesUseCase.Execute(Params);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledgeTypes.Length, result.Value.Count());

            var knowledgeTypeDto = result.Value.First();
            Assert.Equal(knowledgeTypes[0].Id, knowledgeTypeDto.Id);
            Assert.Equal(knowledgeTypes[0].Name, knowledgeTypeDto.Name);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypesAreCached()
        {
            var knowledgeTypes = SeedData.GetKnowledgeTypes();
            var knowledgeTypeDtos = _mapper.Map<IEnumerable<KnowledgeTypeDto>>(knowledgeTypes);

            _cacheMock.Setup(c => c.GetAsync<IEnumerable<KnowledgeTypeDto>>(It.IsAny<string>())).ReturnsAsync(knowledgeTypeDtos);

            var Params = new GetKnowledgeTypesParams { };
            var result = await _getKnowledgeTypesUseCase.Execute(Params);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledgeTypes.Length, result.Value.Count());

            var knowledgeTypeDto = result.Value.First();
            Assert.Equal(knowledgeTypes[0].Id, knowledgeTypeDto.Id);
            Assert.Equal(knowledgeTypes[0].Name, knowledgeTypeDto.Name);
        }
    }
}
