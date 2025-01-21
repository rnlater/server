using Application.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Services
{
    public class MailServiceTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly MailService _mailService;

        public MailServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _mailService = new MailService(_configurationMock.Object);

            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _configurationMock.SetupGet(c => c["Smtp:Name"]).Returns(Configuration["Smtp:Name"]);
            _configurationMock.SetupGet(c => c["Smtp:Email"]).Returns(Configuration["Smtp:Email"]);
            _configurationMock.SetupGet(c => c["Smtp:Server"]).Returns(Configuration["Smtp:Server"]);
            _configurationMock.SetupGet(c => c["Smtp:Port"]).Returns(Configuration["Smtp:Port"]);
            _configurationMock.SetupGet(c => c["Smtp:Pass"]).Returns(Configuration["Smtp:Pass"]);
        }

        // [Fact]
        // public async Task SendEmail_ShouldSendRealEmail_WhenEmailIsSent()
        // {
        //     // Arrange
        //     var to = "boinguyen9701@gmail.com";
        //     var name = "Recipient";
        //     var subject = "Test Subject";
        //     var body = "Test Body";

        //     // Act
        //     var result = await _mailService.SendEmail(to, name, subject, body);

        //     // Assert
        //     Assert.True(result.IsSuccess);
        //     Assert.Equal(to, result.Value);
        // }
    }
}