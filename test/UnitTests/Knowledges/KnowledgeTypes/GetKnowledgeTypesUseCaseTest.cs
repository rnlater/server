using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Application.DTOs;
using Application.UseCases.KnowledgeTypes;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Shared.Types;
using Application.UseCases.Knowledges.KnowledgeTypes;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;

namespace UnitTests.Knowledges.KnowledgeTypes
{
    public class GetKnowledgeTypesUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetKnowledgeTypesUseCase _getKnowledgeTypesUseCase;

        public GetKnowledgeTypesUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);

            _getKnowledgeTypesUseCase = new GetKnowledgeTypesUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTypesFound()
        {
            // Arrange
            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(Enumerable.Empty<KnowledgeType>());

            // Act
            var result = await _getKnowledgeTypesUseCase.Execute(NoParam.Value);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeTypesAreFound()
        {
            // Arrange
            var knowledgeTypes = SeedData.GetKnowledgeTypes();

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeTypes);

            // Act
            var result = await _getKnowledgeTypesUseCase.Execute(NoParam.Value);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(knowledgeTypes.Length, result.Value.Count());

            var knowledgeTypeDto = result.Value.First();
            Assert.Equal(knowledgeTypes[0].Id, knowledgeTypeDto.Id);
            Assert.Equal(knowledgeTypes[0].Name, knowledgeTypeDto.Name);
        }
    }
}
