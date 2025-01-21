using Application.DTOs;
using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Subjects;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;
using System.Security.Claims;
namespace UnitTests.Subjects
{
    public class GetSubjectByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IRedisCache> _cacheMock;
        private readonly IMapper _mapper;
        private readonly GetSubjectByGuidUseCase _getSubjectByGuidUseCase;

        public GetSubjectByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _cacheMock = new Mock<IRedisCache>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);

            _getSubjectByGuidUseCase = new GetSubjectByGuidUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object, _cacheMock.Object);
        }
        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var userId = Guid.NewGuid();

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync((User?)null);

            var result = await _getSubjectByGuidUseCase.Execute(Guid.NewGuid());

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var subjectId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);
            _cacheMock.Setup(c => c.GetAsync<SubjectDto>($"{RedisCache.Keys.GetSubjectByGuid}_{subjectId}")).ReturnsAsync((SubjectDto?)null);

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "", Role = Domain.Enums.Role.Admin });

            var result = await _getSubjectByGuidUseCase.Execute(subjectId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectIsFound()
        {
            var subject = SeedData.GetSubjects()[0];
            var subjectId = subject.Id;
            var userId = Guid.NewGuid();

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _cacheMock.Setup(c => c.GetAsync<SubjectDto>($"{RedisCache.Keys.GetSubjectByGuid}_{subjectId}")).ReturnsAsync((SubjectDto?)null);

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "", Role = Domain.Enums.Role.Admin });

            var result = await _getSubjectByGuidUseCase.Execute(subjectId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(subject.Id, result.Value.Id);
            Assert.Equal(subject.Name, result.Value.Name);
            Assert.Equal(subject.Description, result.Value.Description);
        }
    }
}
