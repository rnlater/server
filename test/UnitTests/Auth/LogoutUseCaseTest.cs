using System.Security.Claims;
using Application.DTOs;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Auth
{
    public class LogoutUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly LogoutUseCase _logoutUseCase;

        public LogoutUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapperMock = new Mock<IMapper>();

            _logoutUseCase = new LogoutUseCase(_unitOfWorkMock.Object, _httpContextAccessorMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenLogoutIsSuccessful()
        {
            var userId = Guid.NewGuid().ToString();
            var user = new User { Id = Guid.Parse(userId), Email = "test@example.com", UserName = "testuser" };
            var authentication = new Authentication
            {
                UserId = Guid.Parse(userId),
                User = user,
                RefreshToken = "token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                IsActivated = true,
                IsEmailConfirmed = true,
                HashedPassword = "hashedpassword"
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) }));

            _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);
            var userAuthenticationRepositoryMock = new Mock<IRepository<Authentication>>();
            userAuthenticationRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Authentication>>())).ReturnsAsync(authentication);
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(userAuthenticationRepositoryMock.Object);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Email = user.Email, UserName = user.UserName });

            var result = await _logoutUseCase.Execute(new NoParam());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Email, result.Value.Email);
            Assert.Equal(user.UserName, result.Value.UserName);
            Assert.Null(authentication.RefreshToken);
            Assert.Null(authentication.RefreshTokenExpiryTime);
            userAuthenticationRepositoryMock.Verify(r => r.Update(It.IsAny<Authentication>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserIdIsNull()
        {
            _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(new ClaimsPrincipal());

            var result = await _logoutUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) }));

            _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);
            var userAuthenticationRepositoryMock = new Mock<IRepository<Authentication>>();
            _ = userAuthenticationRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Authentication>>())).ReturnsAsync((Authentication)null);
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(userAuthenticationRepositoryMock.Object);

            var result = await _logoutUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserAlreadyLoggedOut()
        {
            var userId = Guid.NewGuid().ToString();
            var user = new User { Id = Guid.Parse(userId), Email = "test@example.com", UserName = "testuser" };
            var authentication = new Authentication
            {
                UserId = Guid.Parse(userId),
                User = user,
                RefreshToken = null,
                RefreshTokenExpiryTime = null,
                IsActivated = true,
                IsEmailConfirmed = true,
                HashedPassword = "hashedpassword"
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) }));

            _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);
            var userAuthenticationRepositoryMock = new Mock<IRepository<Authentication>>();
            userAuthenticationRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Authentication>>())).ReturnsAsync(authentication);
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(userAuthenticationRepositoryMock.Object);

            var result = await _logoutUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserAlreadyLoggedOut.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            var userId = Guid.NewGuid().ToString();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId) }));

            _httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(claimsPrincipal);
            var userAuthenticationRepositoryMock = new Mock<IRepository<Authentication>>();
            userAuthenticationRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Authentication>>())).ThrowsAsync(new Exception());
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(userAuthenticationRepositoryMock.Object);

            var result = await _logoutUseCase.Execute(new NoParam());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }
    }
}