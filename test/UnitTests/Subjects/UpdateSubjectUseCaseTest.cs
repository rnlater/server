using System;
using System.Threading.Tasks;
using Xunit;
using Application.DTOs;
using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;
using Shared.Types;
using Application.Interfaces;
using Application.Mappings;
using Microsoft.AspNetCore.Http;
using Domain.Base;

namespace UnitTests.Subjects
{
    public class UpdateSubjectUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly UpdateSubjectUseCase _updateSubjectUseCase;

        public UpdateSubjectUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);

            _updateSubjectUseCase = new UpdateSubjectUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var parameters = new UpdateSubjectParams
            {
                Id = Guid.NewGuid(),
                Name = "Updated Subject",
                Description = "Updated Description",
                Photo = new Mock<IFormFile>().Object
            };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);

            var result = await _updateSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectIsUpdated()
        {
            var subject = SeedData.GetSubjects()[0];
            var parameters = new UpdateSubjectParams
            {
                Id = subject.Id,
                Name = "Updated Subject",
                Description = "Updated Description",
                Photo = new Mock<IFormFile>().Object
            };

            var newPhotoPath = "subjects/newphoto.png";

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(subject.Photo)).Returns(Result<string>.Done(subject.Photo));
            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), "subjects")).ReturnsAsync(Result<string>.Done(newPhotoPath));
            _subjectRepositoryMock.Setup(r => r.Update(It.IsAny<Subject>())).ReturnsAsync(subject);

            var result = await _updateSubjectUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Name, result.Value.Name);
            Assert.Equal(parameters.Description, result.Value.Description);
            Assert.Equal(newPhotoPath, result.Value.Photo);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenDeleteFileFails()
        {
            var subject = SeedData.GetSubjects()[0];
            var parameters = new UpdateSubjectParams
            {
                Id = subject.Id,
                Name = "Updated Subject",
                Description = "Updated Description",
                Photo = new Mock<IFormFile>().Object
            };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _fileStorageServiceMock.Setup(f => f.DeleteFile(subject.Photo)).Returns(Result<string>.Fail(ErrorMessage.DeleteFileError));

            var result = await _updateSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.DeleteFileError, result.Error);
        }
    }
}
