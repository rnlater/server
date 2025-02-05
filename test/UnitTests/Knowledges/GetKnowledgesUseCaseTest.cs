using Application.Mappings;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class GetKnowledgesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgesUseCase _getKnowledgesUseCase;

        public GetKnowledgesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);

            _getKnowledgesUseCase = new GetKnowledgesUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgesFound()
        {
            var parameters = new GetKnowledgesParams
            {
                Search = "",
            };

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(Enumerable.Empty<Knowledge>());

            var result = await _getKnowledgesUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgesAreFound()
        {
            var parameters = new GetKnowledgesParams
            {
                Search = "Introduction"
            };

            var knowledges = SeedData.GetKnowledges();

            _knowledgeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledges);

            var result = await _getKnowledgesUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledges.Length, result.Value.Count());

            var knowledgeDto = result.Value.First();
            Assert.Equal(knowledges[0].Id, knowledgeDto.Id);
            Assert.Equal(knowledges[0].Title, knowledgeDto.Title);
        }
    }
}