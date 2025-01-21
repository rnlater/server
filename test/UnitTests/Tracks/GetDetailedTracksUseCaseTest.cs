using Application.DTOs;
using Application.Interfaces;
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
        private readonly Mock<IRepository<TrackSubject>> _trackSubjectRepositoryMock;
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetDetailedTracksUseCase _getDetailedTracksUseCase;

        public GetDetailedTracksUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _trackSubjectRepositoryMock = new Mock<IRepository<TrackSubject>>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<TrackSubject>()).Returns(_trackSubjectRepositoryMock.Object);

            _getDetailedTracksUseCase = new GetDetailedTracksUseCase(_unitOfWorkMock.Object, _mapper, _cacheMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoTracksFound()
        {
            _cacheMock.Setup(c => c.GetAsync<IEnumerable<TrackDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<TrackDto>?)null);
            _trackRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(Enumerable.Empty<Track>());

            var result = await _getDetailedTracksUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTracksFound.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTracksAreFound()
        {
            var tracks = new List<Track> { SeedData.GetTracks()[0] };
            var trackSubjects = SeedData.GetTrackSubjects().Where(ts => ts.TrackId == SeedData.Track1Id).ToList();

            _cacheMock.Setup(c => c.GetAsync<IEnumerable<TrackDto>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<TrackDto>?)null);
            _trackRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(tracks);
            _trackSubjectRepositoryMock.Setup(r => r.Count(It.IsAny<BaseSpecification<TrackSubject>>()))
                .ReturnsAsync(trackSubjects.Count);

            var result = await _getDetailedTracksUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(tracks.Count, result.Value.Count());

            var trackDto = result.Value.First();
            Assert.Equal(tracks[0].Id, trackDto.Id);
            Assert.Equal(tracks[0].Name, trackDto.Name);
            Assert.Equal(tracks[0].Description, trackDto.Description);
            Assert.Equal(trackSubjects.Count, trackDto.SubjectCount);
        }

        [Fact]
        public async Task Execute_ShouldReturnCachedTracks_WhenCacheIsNotEmpty()
        {
            var cachedTracks = new List<TrackDto> { new TrackDto { Id = SeedData.Track1Id, Name = "Cached Track", Description = "Cached Description", SubjectCount = 1 } };

            _cacheMock.Setup(c => c.GetAsync<IEnumerable<TrackDto>>(It.IsAny<string>())).ReturnsAsync(cachedTracks);

            var result = await _getDetailedTracksUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(cachedTracks.Count, result.Value.Count());

            var trackDto = result.Value.First();
            Assert.Equal(cachedTracks[0].Id, trackDto.Id);
            Assert.Equal(cachedTracks[0].Name, trackDto.Name);
            Assert.Equal(cachedTracks[0].Description, trackDto.Description);
            Assert.Equal(cachedTracks[0].SubjectCount, trackDto.SubjectCount);
        }
    }
}
