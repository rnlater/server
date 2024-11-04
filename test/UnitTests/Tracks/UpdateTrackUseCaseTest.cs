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
    public class UpdateTrackUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UpdateTrackUseCase _updateTrackUseCase;

        public UpdateTrackUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);

            _updateTrackUseCase = new UpdateTrackUseCase(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var parameters = new UpdateTrackParams { Id = Guid.NewGuid(), Name = "Updated Track", Description = "Updated Description" };
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track)null);

            var result = await _updateTrackUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTrackIsUpdated()
        {
            var track = SeedData.GetTracks()[0];
            var parameters = new UpdateTrackParams { Id = track.Id, Name = "Updated Track", Description = "Updated Description" };
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _trackRepositoryMock.Setup(r => r.Update(It.IsAny<Track>())).ReturnsAsync(track);

            var result = await _updateTrackUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(track.Id, result.Value.Id);
            Assert.Equal(parameters.Name, result.Value.Name);
            Assert.Equal(parameters.Description, result.Value.Description);
        }
    }
}