using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Services
{
    public class FileStorageServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly FileStorageService _fileStorageService;
        private readonly string _rootPath = "wwwroot/Upload/Files";

        public FileStorageServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(c => c["FileStorage:RootPath"]).Returns(_rootPath);
            _fileStorageService = new FileStorageService(_configurationMock.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenRootPathIsNotConfigured()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.SetupGet(c => c["FileStorage:RootPath"]).Returns(string.Empty);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new FileStorageService(configurationMock.Object));
        }

        [Fact]
        public void Constructor_ShouldCreateDirectory_WhenRootPathDoesNotExist()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var nonExistentPath = "nonexistent/path";
            configurationMock.SetupGet(c => c["FileStorage:RootPath"]).Returns(nonExistentPath);

            // Act
            var fileStorageService = new FileStorageService(configurationMock.Object);

            // Assert
            Assert.True(Directory.Exists(nonExistentPath));
            Directory.Delete(nonExistentPath);
        }

        [Fact]
        public async Task StoreFile_ShouldStoreFileAndReturnPath()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var directory = "test-directory";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);

            // Act
            var result = await _fileStorageService.StoreFile(fileMock.Object, directory);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains(directory, result.Value);
            Assert.Contains(fileName, result.Value);

            // Clean up
            var fullPath = Path.Combine(_rootPath, result.Value);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        [Fact]
        public void DeleteFile_ShouldDeleteFileAndReturnPath()
        {
            // Arrange
            var filePath = "test-directory/test.txt";
            var fullPath = Path.Combine(_rootPath, filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, "Test content");

            // Act
            var result = _fileStorageService.DeleteFile(filePath);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(filePath, result.Value);
            Assert.False(File.Exists(fullPath));
        }
    }
}