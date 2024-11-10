using Application.UseCases.Subjects;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Infrastructure.Data;
using Moq;
using Shared.Constants;

namespace UnitTests.Subjects
{
    public class CreateDeleteSubjectKnowledgeUseCaseTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Subject>> _subjectRepositoryMock;
        private readonly Mock<IRepository<Knowledge>> _knowledgeRepositoryMock;
        private readonly Mock<IRepository<SubjectKnowledge>> _subjectKnowledgeRepositoryMock;
        private readonly CreateDeleteSubjectKnowledgeUseCase _createDeleteSubjectKnowledgeUseCase;

        public CreateDeleteSubjectKnowledgeUseCaseTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _subjectRepositoryMock = new Mock<IRepository<Subject>>();
            _knowledgeRepositoryMock = new Mock<IRepository<Knowledge>>();
            _subjectKnowledgeRepositoryMock = new Mock<IRepository<SubjectKnowledge>>();

            _unitOfWorkMock.Setup(u => u.Repository<Subject>()).Returns(_subjectRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<Knowledge>()).Returns(_knowledgeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Repository<SubjectKnowledge>()).Returns(_subjectKnowledgeRepositoryMock.Object);

            _createDeleteSubjectKnowledgeUseCase = new CreateDeleteSubjectKnowledgeUseCase(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenSubjectNotFound()
        {
            var parameters = new CreateDeleteSubjectKnowledgeParams { SubjectId = Guid.NewGuid(), KnowledgeId = Guid.NewGuid() };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync((Subject?)null);

            var result = await _createDeleteSubjectKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoSubjectFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnFail_WhenKnowledgeNotFound()
        {
            var subject = SeedData.GetSubjects()[0];
            var parameters = new CreateDeleteSubjectKnowledgeParams { SubjectId = subject.Id, KnowledgeId = Guid.NewGuid() };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync((Knowledge?)null);

            var result = await _createDeleteSubjectKnowledgeUseCase.Execute(parameters);

            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorMessage.NoKnowledgeFoundWithGuid, result.Error);
        }

        [Fact]
        public async Task Execute_ShouldReturnCreated_WhenSubjectKnowledgeDoesNotExist()
        {
            var subject = SeedData.GetSubjects()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var parameters = new CreateDeleteSubjectKnowledgeParams { SubjectId = subject.Id, KnowledgeId = knowledge.Id };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _subjectKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<SubjectKnowledge>>())).ReturnsAsync((SubjectKnowledge?)null);

            var result = await _createDeleteSubjectKnowledgeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Created, result.Value);
            _subjectKnowledgeRepositoryMock.Verify(r => r.Add(It.IsAny<SubjectKnowledge>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnDeleted_WhenSubjectKnowledgeExists()
        {
            var subject = SeedData.GetSubjects()[0];
            var knowledge = SeedData.GetKnowledges()[0];
            var subjectKnowledge = new SubjectKnowledge { SubjectId = subject.Id, KnowledgeId = knowledge.Id };
            var parameters = new CreateDeleteSubjectKnowledgeParams { SubjectId = subject.Id, KnowledgeId = knowledge.Id };

            _subjectRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Subject>>())).ReturnsAsync(subject);
            _knowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<Knowledge>>())).ReturnsAsync(knowledge);
            _subjectKnowledgeRepositoryMock.Setup(r => r.Find(It.IsAny<BaseSpecification<SubjectKnowledge>>())).ReturnsAsync(subjectKnowledge);

            var result = await _createDeleteSubjectKnowledgeUseCase.Execute(parameters);

            Assert.True(result.IsSuccess);
            Assert.Equal(PivotSuccessModificationType.Deleted, result.Value);
            _subjectKnowledgeRepositoryMock.Verify(r => r.Delete(It.IsAny<SubjectKnowledge>()), Times.Once);
        }
    }
}