using Application.DTOs;
using Application.Interfaces;
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
using System.Security.Claims;

namespace UnitTests.Tracks
{
    public class GetTrackByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Track>> _trackRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetTrackByGuidUseCase _getTrackByGuidUseCase;

        public GetTrackByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _trackRepositoryMock = new Mock<IRepository<Track>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Track>()).Returns(_trackRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _getTrackByGuidUseCase = new GetTrackByGuidUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();
            var trackId = Guid.NewGuid();

            SetupHttpContext(userId);
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync((User?)null);

            var result = await _getTrackByGuidUseCase.Execute(trackId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenTrackNotFound()
        {
            var userId = Guid.NewGuid();
            var trackId = Guid.NewGuid();

            SetupHttpContext(userId);
            SetupUser(userId);
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync((Track?)null);
            _cacheMock.Setup(c => c.GetAsync<TrackDto>(It.IsAny<string>())).ReturnsAsync((TrackDto?)null);

            var result = await _getTrackByGuidUseCase.Execute(trackId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoTrackFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenTrackIsFound()
        {
            var userId = Guid.NewGuid();
            var track = SeedData.GetTracks()[0];

            SetupHttpContext(userId);
            SetupUser(userId);
            _trackRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Track>>())).ReturnsAsync(track);
            _cacheMock.Setup(c => c.GetAsync<TrackDto>(It.IsAny<string>())).ReturnsAsync((TrackDto?)null);

            var result = await _getTrackByGuidUseCase.Execute(track.Id);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(track.Id, result.Value.Id);
        }

        private void SetupHttpContext(Guid userId)
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
        }

        private void SetupUser(Guid userId)
        {
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "", Role = Domain.Enums.Role.Admin });
        }
    }
}