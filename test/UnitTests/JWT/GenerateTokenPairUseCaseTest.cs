using Application.DTOs;
using Application.UseCases.JWT;
using AutoMapper;
using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Config;
using Shared.Constants;

namespace UnitTests.JWT;

public class GenerateTokenPairUseCaseTest
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<Authentication>> _tokenRepositoryMock;
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IOptions<JwtSettings> _jwtOptions;
    private readonly GenerateTokenPairUseCase _generateTokenPairUseCase;

    public GenerateTokenPairUseCaseTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tokenRepositoryMock = new Mock<IRepository<Authentication>>();
        _userRepositoryMock = new Mock<IRepository<User>>();
        _mapperMock = new Mock<IMapper>();
        _jwtOptions = Options.Create(new JwtSettings
        {
            SecretKey = "test_secret_key_must_have_at_least_16_chars",
            Issuer = "test_issuer",
            Audience = "test_audience",
            ExpiryMinutes = 180
        });

        _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(_tokenRepositoryMock.Object);
        _generateTokenPairUseCase = new GenerateTokenPairUseCase(_jwtOptions, _unitOfWorkMock.Object);
    }


    [Fact]
    public async Task Execute_ShouldReturnTokens_WhenUserExists()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "test_user", Email = "test_user@example.com" };
        var authentication = new Authentication
        {
            UserId = userDto.Id,
            HashedPassword = "hashed_password_placeholder",
            IsEmailConfirmed = true,
            IsActivated = true
        };
        _tokenRepositoryMock.Setup(tr => tr.Find(It.IsAny<BaseSpecification<Authentication>>()))
            .ReturnsAsync(authentication);

        var result = await _generateTokenPairUseCase.Execute(userDto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value.Item1);
        Assert.NotNull(result.Value.Item2);
        _tokenRepositoryMock.Verify(tr => tr.Update(It.IsAny<Authentication>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnFail_WhenUserDoesNotExist()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "test_user", Email = "test_user@example.com" };
        _tokenRepositoryMock.Setup(tr => tr.Find(It.IsAny<BaseSpecification<Authentication>>()))
            .ReturnsAsync((Authentication)null);

        var result = await _generateTokenPairUseCase.Execute(userDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorMessage.UserNotFound.ToString(), result.Errors[0]);
        _tokenRepositoryMock.Verify(tr => tr.Update(It.IsAny<Authentication>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Execute_ShouldReturnFail_OnException()
    {
        var userDto = new UserDto { Id = Guid.NewGuid(), UserName = "test_user", Email = "test_user@example.com" };
        _tokenRepositoryMock.Setup(tr => tr.Find(It.IsAny<BaseSpecification<Authentication>>()))
            .ThrowsAsync(new Exception());

        var result = await _generateTokenPairUseCase.Execute(userDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorMessage.UnknownError.ToString(), result.Errors[0]);
        _tokenRepositoryMock.Verify(tr => tr.Update(It.IsAny<Authentication>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}
