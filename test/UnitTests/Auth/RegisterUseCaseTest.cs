using AutoMapper;
using Moq;

using Application.DTOs;
using Application.UseCases.Auth;
using Domain.Base;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Utils;
using Domain.Entities.SingleIdEntities;

namespace UnitTests.Auth
{
    public class RegisterUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Authentication>> _authRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RegisterUseCase _registerUseCase;

        public RegisterUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _authRepositoryMock = new Mock<IRepository<Authentication>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(_authRepositoryMock.Object);

            _registerUseCase = new RegisterUseCase(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserAlreadyExists()
        {
            var parameters = new RegisterParams { Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync(new User
                {
                    UserName = "testuser",
                    Email = "test@example.com"
                });

            var result = await _registerUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserAlreadyExistsWithSameEmail.ToString(), result.Errors[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenUserIsRegistered()
        {
            var parameters = new RegisterParams { Email = "test@example.com", Password = "password" };
            var userId = Guid.NewGuid();
            var newUser = new User
            {
                Id = userId,
                Email = parameters.Email,
                UserName = "User-" + Guid.NewGuid().ToString().Substring(0, 8),
            };
            var userAuth = new Authentication
            {
                UserId = userId,
                HashedPassword = PasswordHasher.HashWithSHA256(parameters.Password),
                IsEmailConfirmed = false,
                IsActivated = true,
            };
            newUser.Authentication = userAuth;

            var userDto = new UserDto
            {
                Id = newUser.Id,
                Email = newUser.Email,
                UserName = newUser.UserName,
                Authentication = new AuthenticationDto
                {
                    UserId = userAuth.UserId,
                    HashedPassword = userAuth.HashedPassword,
                    IsEmailConfirmed = userAuth.IsEmailConfirmed,
                    IsActivated = userAuth.IsActivated,
                }
            };

            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ReturnsAsync((User?)null);
            _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>()))
                .ReturnsAsync(newUser);
            _authRepositoryMock.Setup(r => r.Add(It.IsAny<Authentication>()))
                .ReturnsAsync(userAuth);

            _mapperMock.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(userDto);

            var result = await _registerUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(userDto.Id, result.Value.Id);
            Assert.Equal(userDto.Email, result.Value.Email);
            Assert.NotNull(result.Value.Authentication);
            Assert.Equal(userAuth.UserId, result.Value.Authentication.UserId);
            Assert.Equal(userAuth.HashedPassword, result.Value.Authentication.HashedPassword);
            Assert.False(result.Value.Authentication.IsEmailConfirmed);
            Assert.True(result.Value.Authentication.IsActivated);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenExceptionIsThrown()
        {
            var parameters = new RegisterParams { Email = "test@example.com", Password = "password" };
            _userRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<User>>()))
                .ThrowsAsync(new Exception());

            var result = await _registerUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        }
    }
}