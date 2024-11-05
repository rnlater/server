using Application.Mappings;
using Application.UseCases.Tracks;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Tracks
{
    public class GetDetailedTracksUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IRepository<SubjectKnowledge>> _subjectKnowledgeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetDetailedTracksUseCase _getDetailedTracksUseCase;

        public GetDetailedTracksUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _subjectKnowledgeRepositoryMock = new Mock<IRepository<SubjectKnowledge>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<SubjectKnowledge>()).Returns(_subjectKnowledgeRepositoryMock.Object);

            _getDetailedTracksUseCase = new GetDetailedTracksUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoTracksFound()
        {
            _trackRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(Enumerable.Empty<Track>());

            var result = await _getDetailedTracksUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFound.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTracksAreFound()
        {
            var subjectKnowledges = SeedData.GetSubjectKnowledges().Where(sk => sk.SubjectId == SeedData.Subject1Id).ToList();
            var trackSubjects = SeedData.GetTrackSubjects().Where(ts => ts.TrackId == SeedData.Track1Id && ts.SubjectId == SeedData.Subject1Id).ToList();
            Subject subject = SeedData.GetSubjects().First(s => s.Id == SeedData.Subject1Id);
            subject.SubjectKnowledges = subjectKnowledges;
            trackSubjects[0].Subject = subject;
            var tracks = new List<Track> { SeedData.GetTracks()[0] };
            tracks[0].TrackSubjects = trackSubjects;

            _trackRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(tracks);
            _subjectKnowledgeRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<SubjectKnowledge>>()))
                .ReturnsAsync(subjectKnowledges.Count);

            var result = await _getDetailedTracksUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(tracks.Count, result.Value.Count());

            var trackDto = result.Value.First();
            Assert.Equal(tracks[0].Id, trackDto.Id);
            Assert.Equal(tracks[0].Name, trackDto.Name);
            Assert.Equal(tracks[0].Description, trackDto.Description);

            var trackSubjectDto = trackDto.TrackSubjects.First();
            Assert.Equal(trackSubjects[0].SubjectId, trackSubjectDto.SubjectId);
            Assert.Equal(subjectKnowledges.Count, trackSubjectDto.Subject!.KnowledgeCount);
        }
    }
}