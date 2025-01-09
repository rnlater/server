using Application.Mappings;
using Application.UseCases.Tracks;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Tracks
{
    public class GetTrackByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly GetTrackByGuidUseCase _getTrackByGuidUseCase;

        public GetTrackByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);

            _getTrackByGuidUseCase = new GetTrackByGuidUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var trackId = Guid.NewGuid();
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track?)null);

            var result = await _getTrackByGuidUseCase.Execute(trackId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTrackIsFound()
        {
            var track = SeedData.GetTracks()[0];
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);

            var result = await _getTrackByGuidUseCase.Execute(track.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(track.Id, result.Value.Id);
        }
    }
}