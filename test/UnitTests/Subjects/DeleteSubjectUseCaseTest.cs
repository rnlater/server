using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;
using Shared.Types;
using Application.Mappings;
using Domain.Base;
using Infrastructure.Data;
using Application.Interfaces;

namespace UnitTests.Subjects
{
    public class DeleteSubjectUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly DeleteSubjectUseCase _deleteSubjectUseCase;

        public DeleteSubjectUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);

            _deleteSubjectUseCase = new DeleteSubjectUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var subjectId = Guid.NewGuid();

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);

            var result = await _deleteSubjectUseCase.Execute(subjectId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectIsDeleted()
        {
            var subject = SeedData.GetSubjects()[0];

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _subjectRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(subject);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Done("photo.png"));

            var result = await _deleteSubjectUseCase.Execute(subject.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(subject.Id, result.Value.Id);
            Assert.Equal(subject.Name, result.Value.Name);
            _fileStorageServiceMock.Verify(f => f.DeleteFile(subject.Photo), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenDeleteFileFails()
        {
            var subject = SeedData.GetSubjects()[0];

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _subjectRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(subject);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Result<string>.Fail(ErrorMessage.DeleteFileError));

            var result = await _deleteSubjectUseCase.Execute(subject.Id);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.DeleteFileError, result.Error);
        }
    }
}
