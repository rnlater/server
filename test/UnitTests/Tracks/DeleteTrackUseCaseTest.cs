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
    public class DeleteTrackUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly IMapper _mapper;
        private readonly DeleteTrackUseCase _deleteTrackUseCase;

        public DeleteTrackUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);

            _deleteTrackUseCase = new DeleteTrackUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var trackId = Guid.NewGuid();
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track?)null);

            var result = await _deleteTrackUseCase.Execute(trackId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTrackIsDeleted()
        {
            var track = SeedData.GetTracks()[0];
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _trackRepositoryMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(track);

            var result = await _deleteTrackUseCase.Execute(SeedData.Track1Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(track.Id, result.Value.Id);
        }
    }
}