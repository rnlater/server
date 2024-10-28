using Application.DTOs;
using Application.UseCases.JWT;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Config;
using Shared.Constants;

namespace UnitTests.JWT
{
    public class RenewAccessTokenUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IOptions<JwtSettings>> _jwtOptionsMock;
        private readonly RenewAccessTokenUseCase _renewAccessTokenUseCase;

        public RenewAccessTokenUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
            _jwtOptionsMock.Setup(o => o.Value).Returns(new JwtSettings
            {
                SecretKey = "test_secret_key_must_have_at_least_16_chars",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpiryMinutes = 180
            });
            _renewAccessTokenUseCase = new RenewAccessTokenUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _jwtOptionsMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnAccessToken_WhenRefreshTokenIsValid()
        {
            var refreshToken = "valid_refresh_token";
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = "testuser@example.com",
                Role = Role.User
            };
            var authentication = new Authentication
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(5),
                User = user,
                HashedPassword = "hashed_password",
                IsEmailConfirmed = true,
                IsActivated = true,
                UserId = user.Id
            };
            user.Authentication = authentication;

            _unitOfWorkMock
                .Setup(uow => uow.Repository<Authentication>().Find(It.IsAny<BaseSpecification<Authentication>>()))
                .ReturnsAsync(authentication);

            _mapperMock
                .Setup(m => m.Map<UserDto>(user))
                .Returns(new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role });

            var result = await _renewAccessTokenUseCase.Execute(refreshToken);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }


        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var refreshToken = "invalid_refresh_token";

            _unitOfWorkMock
                .Setup(uow => uow.Repository<Authentication>().Find(It.IsAny<BaseSpecification<Authentication>>()))
                .ReturnsAsync((Authentication)null);

            var result = await _renewAccessTokenUseCase.Execute(refreshToken);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenRefreshTokenIsExpired()
        {
            var refreshToken = "expired_refresh_token";
            var user = new User { Id = Guid.NewGuid(), UserName = "testuser", Email = "testuser@example.com" };
            var authentication = new Authentication
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-5),
                User = user,
                HashedPassword = "hashed_password",
                IsEmailConfirmed = true,
                IsActivated = true,
                UserId = user.Id
            };

            _unitOfWorkMock
                .Setup(uow => uow.Repository<Authentication>().Find(It.IsAny<BaseSpecification<Authentication>>()))
                .ReturnsAsync(authentication);

            var result = await _renewAccessTokenUseCase.Execute(refreshToken);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.RefreshTokenIsExpired.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            var refreshToken = "valid_refresh_token";

            _unitOfWorkMock
                .Setup(uow => uow.Repository<Authentication>().Find(It.IsAny<BaseSpecification<Authentication>>()))
                .ThrowsAsync(new Exception());

            var result = await _renewAccessTokenUseCase.Execute(refreshToken);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }
    }
}
