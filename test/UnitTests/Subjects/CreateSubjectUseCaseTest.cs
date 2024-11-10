using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;
using Shared.Types;

namespace UnitTests.Subjects
{
    public class CreateSubjectUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly IMapper _mapper;
        private readonly CreateSubjectUseCase _createSubjectUseCase;

        public CreateSubjectUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);

            _createSubjectUseCase = new CreateSubjectUseCase(_unitOfWorkMock.Object, _mapper, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var parameters = new CreateSubjectParams
            {
                Name = "New Subject",
                Description = "Description",
                Photo = new Mock<IFormFile>().Object,
                TrackUids = new List<Guid> { Guid.NewGuid() },
                KnowledgeUids = new List<Guid> { SeedData.Knowledge1Id }
            };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track?)null);

            var result = await _createSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            var parameters = new CreateSubjectParams
            {
                Name = "New Subject",
                Description = "Description",
                Photo = new Mock<IFormFile>().Object,
                TrackUids = new List<Guid> { SeedData.Track1Id },
                KnowledgeUids = new List<Guid> { Guid.NewGuid() }
            };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(SeedData.GetTracks()[0]);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            var result = await _createSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenPhotoStorageFails()
        {
            var parameters = new CreateSubjectParams
            {
                Name = "New Subject",
                Description = "Description",
                Photo = new Mock<IFormFile>().Object,
                TrackUids = new List<Guid> { SeedData.Track1Id },
                KnowledgeUids = new List<Guid> { SeedData.Knowledge1Id }
            };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(SeedData.GetTracks()[0]);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(SeedData.GetKnowledges()[0]);
            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), "subjects")).ReturnsAsync(Result<string>.Fail(ErrorMessage.UnknownError));

            var result = await _createSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectIsCreated()
        {
            var parameters = new CreateSubjectParams
            {
                Name = "New Subject",
                Description = "Description",
                Photo = new Mock<IFormFile>().Object,
                TrackUids = new List<Guid> { SeedData.Track1Id },
                KnowledgeUids = new List<Guid> { SeedData.Knowledge1Id }
            };

            var track = SeedData.GetTracks()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var photoPath = "subjects/photo.png";

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _fileStorageServiceMock.Setup(f => f.StoreFile(It.IsAny<IFormFile>(), "subjects")).ReturnsAsync(Result<string>.Done(photoPath));

            var result = await _createSubjectUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Name, result.Value.Name);
            Assert.Equal(parameters.Description, result.Value.Description);
            Assert.Equal(photoPath, result.Value.Photo);
        }
    }
}
