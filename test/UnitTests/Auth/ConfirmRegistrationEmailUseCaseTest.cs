using AutoMapper;
using Moq;

using Application.DTOs;
using Application.UseCases.Auth;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Constants;

namespace UnitTests.Auth
{
    public class ConfirmRegistrationEmailUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ConfirmRegistrationEmailUseCase _confirmRegistrationEmailUseCase;

        public ConfirmRegistrationEmailUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _confirmRegistrationEmailUseCase = new ConfirmRegistrationEmailUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync((User?)null);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFoundWithEmail.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenEmailAlreadyConfirmed()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = true,
                    IsActivated = true,
                    ConfirmationCode = "123456",
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10)
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
            .ReturnsAsync(user);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.EmailAlreadyConfirmed.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenConfirmationCodeIsNull()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsActivated = true,
                    IsEmailConfirmed = false,
                    ConfirmationCode = null,
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10)
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
            .ReturnsAsync(user);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.EmailAlreadyConfirmed.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenConfirmationCodeExpiryTimeIsNull()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsActivated = true,
                    IsEmailConfirmed = false,
                    ConfirmationCode = "123456",
                    ConfirmationCodeExpiryTime = null
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
            .ReturnsAsync(user);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.EmailAlreadyConfirmed.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenInvalidConfirmationCode()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "wrongcode" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsEmailConfirmed = false,
                    ConfirmationCode = "123456",
                    IsActivated = true,
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10)
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync(user);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.InvalidConfirmationCode.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenConfirmationCodeExpired()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsActivated = true,
                    IsEmailConfirmed = false,
                    ConfirmationCode = "123456",
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(-10)
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync(user);

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.ConfirmationCodeExpired.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenConfirmationIsSuccessful()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            var user = new User
            {
                Email = parameters.Email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword",
                    IsActivated = true,
                    IsEmailConfirmed = false,
                    ConfirmationCode = "123456",
                    ConfirmationCodeExpiryTime = DateTime.UtcNow.AddMinutes(10)
                }
            };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Email = user.Email, UserName = user.UserName });

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Email, result.Value.Email);
            Assert.Equal(user.UserName, result.Value.UserName);
            Assert.True(user.Authentication.IsEmailConfirmed);
            Assert.Null(user.Authentication.ConfirmationCode);
            Assert.Null(user.Authentication.ConfirmationCodeExpiryTime);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            var parameters = new ConfirmRegistrationEmailParams { Email = "test@example.com", ConfirmationCode = "123456" };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ThrowsAsync(new Exception());

            var result = await _confirmRegistrationEmailUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }
    }
}