using Application.UseCases.JWT;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Config;

namespace UnitTests.JWT
{
    public class GenerateTokenPairUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Authentication>> _tokenRepositoryMock;
        private readonly IOptions<JwtSettings> _jwtOptions;
        private readonly GenerateTokenPairUseCase _generateTokenPairUseCase;

        public GenerateTokenPairUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tokenRepositoryMock = new Mock<IRepository<Authentication>>();
            _jwtOptions = Options.Create(new JwtSettings
            {
                SecretKey = "test_secret_key_must_have_at_least_16_chars",
                Issuer = "test_issuer",
                Audience = "test_audience",
                ExpiryMinutes = 180
            });

            _unitOfWorkMock.Setup(u => u.Repository<Authentication>()).Returns(_tokenRepositoryMock.Object);
            _generateTokenPairUseCase = new GenerateTokenPairUseCase(_jwtOptions);
        }

        [Fact]
        public async Task Execute_ShouldReturnTokens_WhenUserExists()
        {
            var user = new User { Id = Guid.NewGuid(), UserName = "test_user", Email = "test_user@example.com" };

            var result = await _generateTokenPairUseCase.Execute(user);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.False(string.IsNullOrEmpty(result.Value.AccessToken));
            Assert.False(string.IsNullOrEmpty(result.Value.RefreshToken));
        }
    }
}
