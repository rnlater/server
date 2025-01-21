using System.Security.Claims;
using Application.Interfaces;
using Application.Mappings;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Shared.Constants;

namespace UnitTests.Knowledges
{
    public class CreateKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IHttpContextAccessor> _httpcontextAccessorMock;
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeType>> _knowledgeTypeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTypeKnowledge>> _knowledgeTypeKnowledgeRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopic>> _knowledgeTopicRepositoryMock;
        private readonly Mock<IRepository<KnowledgeTopicKnowledge>> _knowledgeTopicKnowledgeRepositoryMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IRepository<Material>> _materialRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly CreateKnowledgeUseCase _createKnowledgeUseCase;

        public CreateKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _httpcontextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IRepository<User>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _knowledgeTypeRepositoryMock = new Mock<IRepository<KnowledgeType>>();
            _knowledgeTypeKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTypeKnowledge>>();
            _knowledgeTopicRepositoryMock = new Mock<IRepository<KnowledgeTopic>>();
            _knowledgeTopicKnowledgeRepositoryMock = new Mock<IRepository<KnowledgeTopicKnowledge>>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _materialRepositoryMock = new Mock<IRepository<Material>>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeType>()).Returns(_knowledgeTypeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTypeKnowledge>()).Returns(_knowledgeTypeKnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopic>()).Returns(_knowledgeTopicRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<KnowledgeTopicKnowledge>()).Returns(_knowledgeTopicKnowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Material>()).Returns(_materialRepositoryMock.Object);

            _createKnowledgeUseCase = new CreateKnowledgeUseCase(_unitOfWorkMock.Object, _mapper, _httpcontextAccessorMock.Object, _fileStorageServiceMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTypesFound()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                KnowledgeTypeIds = [Guid.NewGuid()]
            };

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(Enumerable.Empty<KnowledgeType>());

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTypesFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoKnowledgeTopicsFound()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                KnowledgeTopicIds = [Guid.NewGuid()]
            };

            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(Enumerable.Empty<KnowledgeTopic>());

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeTopicsFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoSubjectsFound()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                SubjectIds = [Guid.NewGuid()]
            };

            _subjectRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(Enumerable.Empty<Subject>());

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectsFound, result.Error);
        }


        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotFound()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge"
            };

            _httpcontextAccessorMock.Setup(h => h.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())])));
            _userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((User?)null);

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotFound, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenUserNotAuthorized()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                SubjectIds = [Guid.NewGuid()]
            };

            var user = new User { Id = Guid.NewGuid(), Role = Role.User, Email = "", UserName = "" };
            _httpcontextAccessorMock.Setup(h => h.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, user.Id.ToString())])));
            _userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(user);
            _subjectRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(new List<Subject> { new() { Id = Guid.NewGuid(), Name = "", Photo = "", Description = "" } });

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.UserNotAuthorized, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenNoInterpretationForKnowledge()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                Materials = [new() { Type = MaterialType.TextMedium, Content = "Content" }]
            };

            var user = new User { Id = Guid.NewGuid(), Role = Role.Admin, Email = "", UserName = "" };
            _httpcontextAccessorMock.Setup(h => h.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, user.Id.ToString())])));
            _userRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(user);

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoInterpretationForKnowledge, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnSuccess_WhenKnowledgeIsCreated()
        {
            var parameters = new CreateKnowledgeParams
            {
                Title = "Test Knowledge",
                Level = KnowledgeLevel.Beginner,
                KnowledgeTypeIds = new List<Guid> { Guid.NewGuid() },
                KnowledgeTopicIds = new List<Guid> { Guid.NewGuid() },
                SubjectIds = new List<Guid>(),
                Materials = new List<CreateMaterialParams>
                {
                    new CreateMaterialParams
                    {
                    Type = MaterialType.Interpretation,
                    Content = "Content",
                    Order = 1,
                    Children = new List<CreateMaterialParams>
                    {
                        new CreateMaterialParams
                        {
                        Type = MaterialType.TextSmall,
                        Content = "Child Content",
                        Order = 1
                        }
                    }
                    }
                }
            };

            var knowledgeTypes = new List<KnowledgeType> { new() { Id = parameters.KnowledgeTypeIds[0], Name = "Type 1" } };
            var knowledgeTopics = new List<KnowledgeTopic> { new() { Id = parameters.KnowledgeTopicIds[0], Title = "Topic 1" } };
            var subjects = new List<Subject>();

            _knowledgeTypeRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeType>>())).ReturnsAsync(knowledgeTypes);
            _knowledgeTopicRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<KnowledgeTopic>>())).ReturnsAsync(knowledgeTopics);
            _subjectRepositoryMock.Setup(r => r.FindMany(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subjects);
            var userId = Guid.NewGuid();
            _httpcontextAccessorMock.Setup(h => h.HttpContext!.User.FindFirst(It.IsAny<string>())).Returns(new System.Security.Claims.Claim("sub", userId.ToString()));
            _userRepositoryMock.Setup(r => r.GetById(userId)).ReturnsAsync(new User { Id = userId, Email = "", UserName = "" });
            var knowledgeId = Guid.NewGuid();

            _knowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<Knowledge>())).ReturnsAsync(new Knowledge { Id = knowledgeId, Title = parameters.Title });
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(new Knowledge { Id = knowledgeId, Title = parameters.Title, Materials = new List<Material> { new() { Type = MaterialType.Interpretation, Content = "Content", Order = 1, Children = new List<Material> { new() { Type = MaterialType.TextSmall, Content = "Child Content", Order = 1 } } } } });
            _knowledgeTypeKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeTypeKnowledge>())).ReturnsAsync(new KnowledgeTypeKnowledge());
            _knowledgeTopicKnowledgeRepositoryMock.Setup(r => r.Add(It.IsAny<KnowledgeTopicKnowledge>())).ReturnsAsync(new KnowledgeTopicKnowledge());

            var _gameRepositoryMock = new Mock<IRepository<Game>>();
            _unitOfWorkMock.Setup(u => u.Repository<Game>()).Returns(_gameRepositoryMock.Object);
            _gameRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Game>>())).ReturnsAsync((Game?)null);

            var result = await _createKnowledgeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(parameters.Title, result.Value.Title);
        }
    }
}