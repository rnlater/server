
using Moq;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Interfaces;
using Shared.Constants;
using Application.UseCases.Tracks;
using Infrastructure.Data;
using Domain.Entities.SingleIdEntities;

namespace UnitTests.Tracks
{
    public class CreateDeleteTrackSubjectUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<TrackSubject>> _trackSubjectRepositoryMock;
        private readonly CreateDeleteTrackSubjectUseCase _createDeleteTrackSubjectUseCase;

        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;

        public CreateDeleteTrackSubjectUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _trackSubjectRepositoryMock = new Mock<IRepository<TrackSubject>>();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<TrackSubject>()).Returns(_trackSubjectRepositoryMock.Object);

            _createDeleteTrackSubjectUseCase = new CreateDeleteTrackSubjectUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var subject = SeedData.GetSubjects()[0];
            var parameters = new CreateDeleteTrackSubjectParams { TrackId = Guid.NewGuid(), SubjectId = subject.Id };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track?)null);
            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);

            var result = await _createDeleteTrackSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var track = SeedData.GetTracks()[0];
            var parameters = new CreateDeleteTrackSubjectParams { TrackId = track.Id, SubjectId = Guid.NewGuid() };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);

            var result = await _createDeleteTrackSubjectUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnCreated_WhenTrackSubjectDoesNotExist()
        {
            var track = SeedData.GetTracks()[0];
            var subject = SeedData.GetSubjects()[0];

            var parameters = new CreateDeleteTrackSubjectParams { TrackId = track.Id, SubjectId = subject.Id };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _trackSubjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<TrackSubject>>())).ReturnsAsync((TrackSubject?)null);

            var result = await _createDeleteTrackSubjectUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Created, result.Value);
            _trackSubjectRepositoryMock.Verify(r => r.Add(It.IsAny<TrackSubject>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnDeleted_WhenTrackSubjectExists()
        {
            var track = SeedData.GetTracks()[0];
            var subject = SeedData.GetSubjects()[0];
            var trackSubject = new TrackSubject { TrackId = track.Id, SubjectId = subject.Id };

            var parameters = new CreateDeleteTrackSubjectParams { TrackId = track.Id, SubjectId = subject.Id };

            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _trackSubjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<TrackSubject>>())).ReturnsAsync(trackSubject);

            var result = await _createDeleteTrackSubjectUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Deleted, result.Value);
            _trackSubjectRepositoryMock.Verify(r => r.Delete(It.IsAny<TrackSubject>()), Times.Once);
        }
    }
}