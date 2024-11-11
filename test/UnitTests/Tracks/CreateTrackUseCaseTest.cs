using AutoMapper;
using Moq;
using Application.DTOs;
using Application.UseCases.Tracks;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Infrastructure.Data;

namespace UnitTests.Tracks
{
    public class CreateTrackUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IRepository<TrackSubject>> _trackSubjectRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateTrackUseCase _createTrackUseCase;

        public CreateTrackUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _trackSubjectRepositoryMock = new Mock<IRepository<TrackSubject>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<TrackSubject>()).Returns(_trackSubjectRepositoryMock.Object);

            _createTrackUseCase = new CreateTrackUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var parameters = new CreateTrackParams
            {
                Name = "New Track",
                Description = "Description",
                SubjectGuids = [Guid.NewGuid()]
            };

            _subjectRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Subject?)null);

            var result = await _createTrackUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTrackIsCreated()
        {
            var parameters = new CreateTrackParams
            {
                Name = "New Track",
                Description = "Description",
                SubjectGuids = [SeedData.Subject1Id, SeedData.Subject2Id]
            };

            var track = new Track { Id = Guid.NewGuid(), Name = parameters.Name, Description = parameters.Description };
            var subjects = SeedData.GetSubjects().ToList();

            _trackRepositoryMock.Setup(r => r.Add(It.IsAny<Track>())).ReturnsAsync(track);
            _subjectRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Guid id) => subjects.FirstOrDefault(s => s.Id == id));
            _trackSubjectRepositoryMock.Setup(r => r.Add(It.IsAny<TrackSubject>())).ReturnsAsync(new TrackSubject());

            var trackDto = new TrackDto { Id = track.Id, Name = track.Name, Description = track.Description };
            _mapperMock.Setup(m => m.Map<TrackDto>(It.IsAny<Track>())).Returns(trackDto);

            var result = await _createTrackUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(trackDto.Id, result.Value.Id);
            Assert.Equal(trackDto.Name, result.Value.Name);
            Assert.Equal(trackDto.Description, result.Value.Description);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoSubjectFoundWithGuidExceptionIsThrown()
        {
            var parameters = new CreateTrackParams
            {
                Name = "New Track",
                Description = "Description",
                SubjectGuids = [SeedData.Subject1Id, SeedData.Subject2Id]
            };

            var track = new Track { Id = Guid.NewGuid(), Name = parameters.Name, Description = parameters.Description };
            List<Subject> subjects = [SeedData.GetSubjects()[0]];

            _trackRepositoryMock.Setup(r => r.Add(It.IsAny<Track>())).ReturnsAsync(track);
            _subjectRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Guid id) => subjects.FirstOrDefault(s => s.Id == id));

            var result = await _createTrackUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid.ToString(), result.Errors[0]);
        }


    }
}