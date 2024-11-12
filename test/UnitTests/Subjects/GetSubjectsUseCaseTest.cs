using Application.Mappings;
using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Subjects
{
    public class GetSubjectsUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetSubjectsUseCase _getSubjectsUseCase;

        public GetSubjectsUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);

            _getSubjectsUseCase = new GetSubjectsUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoSubjectsFound()
        {
            var parameters = new GetSubjectsParams { Search = "NonExistentSubject" };

            _subjectRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(Enumerable.Empty<Subject>());

            var result = await _getSubjectsUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectsFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectsAreFound()
        {
            var parameters = new GetSubjectsParams { Search = "Mathematics" };
            var subjects = SeedData.GetSubjects().Where(s => s.Name.Contains(parameters.Search)).ToList();

            _subjectRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subjects);

            var result = await _getSubjectsUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(subjects.Count, result.Value.Count());

            var subjectDto = result.Value.First();
            Assert.Equal(subjects[0].Id, subjectDto.Id);
            Assert.Equal(subjects[0].Name, subjectDto.Name);
            Assert.Equal(subjects[0].Description, subjectDto.Description);
        }
    }
}
