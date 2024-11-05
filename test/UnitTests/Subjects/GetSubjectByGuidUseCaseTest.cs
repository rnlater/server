using Application.DTOs;
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
using Shared.Types;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Subjects
{
    public class GetSubjectByGuidUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IMapper _mapper;
        private readonly GetSubjectByGuidUseCase _getSubjectByGuidUseCase;

        public GetSubjectByGuidUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);

            _getSubjectByGuidUseCase = new GetSubjectByGuidUseCase(_unitOfWorkMock.Object, _mapper, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var subjectId = Guid.NewGuid();

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);

            var result = await _getSubjectByGuidUseCase.Execute(subjectId);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenSubjectIsFound()
        {
            var subject = SeedData.GetSubjects()[0];
            var subjectId = subject.Id;

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);

            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });

            var result = await _getSubjectByGuidUseCase.Execute(subjectId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(subject.Id, result.Value.Id);
            Assert.Equal(subject.Name, result.Value.Name);
            Assert.Equal(subject.Description, result.Value.Description);
        }
    }
}
