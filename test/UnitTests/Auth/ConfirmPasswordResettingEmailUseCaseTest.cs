using Application.DTOs;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Auth
{
    public class ConfirmPasswordResettingEmailUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ConfirmPasswordResettingEmailUseCase _confirmPasswordResettingEmailUseCase;

        public ConfirmPasswordResettingEmailUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _confirmPasswordResettingEmailUseCase = new ConfirmPasswordResettingEmailUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenConfirmationIsSuccessful()
        {
            var parameters = new ConfirmPasswordResettingEmailParams
            {
                Email = "test@example.com",
                ConfirmationCode = "validCode",
                Password = "newPassword"
            };
            var user = new User
            {
                UserName = "testuser",
                Email = parameters.Email,
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = true,
                    ConfirmationCode = parameters.ConfirmationCode,
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10),
                    IsActivated = true
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);
            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Email = user.Email, UserName = user.UserName });

            var result = await _confirmPasswordResettingEmailUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Email, result.Value.Email);
            Assert.Null(user.Authentication.ConfirmationCode);
            Assert.Null(user.Authentication.ConfirmationCodeExpiryTime);
            userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new ConfirmPasswordResettingEmailParams
            {
                Email = "test@example.com",
                ConfirmationCode = "validCode",
                Password = "newPassword"
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync((User)null);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _confirmPasswordResettingEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFoundWithEmail.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenConfirmationCodeIsInvalid()
        {
            var parameters = new ConfirmPasswordResettingEmailParams
            {
                Email = "test@example.com",
                ConfirmationCode = "invalidCode",
                Password = "newPassword"
            };
            var user = new User
            {
                UserName = "testuser",
                Email = parameters.Email,
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = true,
                    ConfirmationCode = "validCode",
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10),
                    IsActivated = true
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _confirmPasswordResettingEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.InvalidConfirmationCode.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenConfirmationCodeIsExpired()
        {
            var parameters = new ConfirmPasswordResettingEmailParams
            {
                Email = "test@example.com",
                ConfirmationCode = "validCode",
                Password = "newPassword"
            };
            var user = new User
            {
                UserName = "testuser",
                Email = parameters.Email,
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = true,
                    ConfirmationCode = parameters.ConfirmationCode,
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(-10),
                    IsActivated = true
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _confirmPasswordResettingEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.ConfirmationCodeExpired.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenAccountIsLocked()
        {
            var parameters = new ConfirmPasswordResettingEmailParams
            {
                Email = "test@example.com",
                ConfirmationCode = "validCode",
                Password = "newPassword"
            };
            var user = new User
            {
                UserName = "testuser",
                Email = parameters.Email,
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = true,
                    ConfirmationCode = parameters.ConfirmationCode,
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10),
                    IsActivated = false // Account is locked
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _confirmPasswordResettingEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.AccountIsLocked.ToString(), result.Errors[0]);
        }
    }
}
