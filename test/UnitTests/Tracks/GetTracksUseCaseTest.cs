using Application.Mappings;
using Application.UseCases.Tracks;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Tracks
{
    public class GetTracksUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetTracksUseCase _getTracksUseCase;

        public GetTracksUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);

            _getTracksUseCase = new GetTracksUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoTracksFound()
        {
            var parameters = new GetTracksParams { Search = "NonExistentTrack" };
            _trackRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(Enumerable.Empty<Track>());

            var result = await _getTracksUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithSearch.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTracksAreFound()
        {
            var parameters = new GetTracksParams { Search = "Mathematics" };
            var tracks = new List<Track> { SeedData.GetTracks()[0] };
            _trackRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(tracks);

            var result = await _getTracksUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            Assert.Equal(tracks.Count, result.Value.Count());
        }
    }

}
