using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Auth;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Moq;
using Shared.Constants;

namespace UnitTests.Auth
{
    public class ForgotPasswordUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMailService> _mailServiceMock;
        private readonly ForgotPasswordUseCase _forgotPasswordUseCase;

        public ForgotPasswordUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _mailServiceMock = new Mock<IMailService>();

            _forgotPasswordUseCase = new ForgotPasswordUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _mailServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenUserExistsAndEmailConfirmed()
        {
            var email = "test@example.com";
            var user = new User
            {
                Email = email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    IsEmailConfirmed = true,
                    IsActivated = true,
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword"
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            var _authenticationRepositoryMock = new Mock<IRepository<Authentication>>();
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(_authenticationRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>())).Returns(new UserDto { Email = user.Email, UserName = user.UserName });

            var result = await _forgotPasswordUseCase.Execute(new ForgotPasswordParams { Email = email });

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(user.Email, result.Value.Email);
            _authenticationRepositoryMock.Verify(r => r.Update(It.IsAny<Authentication>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var email = "test@example.com";

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync((User?)null);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _forgotPasswordUseCase.Execute(new ForgotPasswordParams { Email = email });

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFoundWithEmail.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenEmailNotConfirmed()
        {
            var email = "test@example.com";
            var user = new User
            {
                Email = email,
                UserName = "testuser",
                Authentication = new Authentication
                {
                    IsEmailConfirmed = false,
                    IsActivated = true,
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword"
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _forgotPasswordUseCase.Execute(new ForgotPasswordParams { Email = email });

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.EmailNotConfirmed.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenAccountIsLocked()
        {
            var email = "test@example.com";
            var user = new User
            {
                UserName = "testuser",
                Email = email,
                Authentication = new Authentication
                {
                    IsEmailConfirmed = true,
                    IsActivated = false,
                    UserId = Guid.NewGuid(),
                    HashedPassword = "hashedpassword"
                }
            };

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _forgotPasswordUseCase.Execute(new ForgotPasswordParams { Email = email });

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.AccountIsLocked.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            var email = "test@example.com";

            var userRepositoryMock = new Mock<IRepository<User>>();
            userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>())).ThrowsAsync(new Exception());
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepositoryMock.Object);

            var result = await _forgotPasswordUseCase.Execute(new ForgotPasswordParams { Email = email });

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }
    }
}