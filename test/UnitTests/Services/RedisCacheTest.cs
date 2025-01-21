using System.Text.Json;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using StackExchange.Redis;

namespace UnitTests.Services
{
    public class RedisCacheTest
    {
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly RedisCache _redisCache;

        public RedisCacheTest()
        {
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _redisMock = new Mock<IConnectionMultiplexer>();
            _databaseMock = new Mock<IDatabase>();
            _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_databaseMock.Object);
            _redisCache = new RedisCache(_redisMock.Object);
        }

        [Fact]
        public async Task SetStringAsync_ShouldSetStringInRedis()
        {
            // Arrange
            var key = "test-key";
            var value = "test-value";
            var expiry = TimeSpan.FromMinutes(5);

            // Act
            await _redisCache.SetStringAsync(key, value, expiry);

            // Assert
            _databaseMock.Verify(db => db.StringSetAsync(key, value, expiry, It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task GetStringAsync_ShouldReturnStringFromRedis()
        {
            // Arrange
            var key = "test-key";
            var value = "test-value";
            _databaseMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>())).ReturnsAsync(value);

            // Act
            var result = await _redisCache.GetStringAsync(key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public async Task SetAsync_ShouldSetObjectInRedis()
        {
            // Arrange
            var key = "test-key";
            var value = new { Name = "Test" };
            var expiry = TimeSpan.FromMinutes(5);

            // Act
            await _redisCache.SetAsync(key, value, expiry);

            // Assert
            _databaseMock.Verify(db => db.StringSetAsync(key, It.IsAny<RedisValue>(), expiry, It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnObjectFromRedis()
        {
            // Arrange
            var key = "test-key";
            var value = new TestObject { Name = "Test" };
            var json = JsonSerializer.Serialize(value);
            _databaseMock.Setup(db => db.StringGetAsync(key, It.IsAny<CommandFlags>())).ReturnsAsync(json);

            // Act
            var result = await _redisCache.GetAsync<TestObject>(key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(value.Name, result.Name);
        }

        private class TestObject
        {
            public required string Name { get; set; }
        }
    }
}