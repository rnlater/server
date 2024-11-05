using System;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Subjects
{
    public class CreateDeleteSubjectKnowledgeParams
    {
        public Guid SubjectId { get; set; }
        public Guid KnowledgeId { get; set; }
    }

    public class CreateDeleteSubjectKnowledgeUseCase : IUseCase<PivotSuccessModificationType, CreateDeleteSubjectKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateDeleteSubjectKnowledgeUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PivotSuccessModificationType>> Execute(CreateDeleteSubjectKnowledgeParams parameters)
        {
            try
            {
                var subjectRepository = _unitOfWork.Repository<Subject>();
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var subjectKnowledgeRepository = _unitOfWork.Repository<SubjectKnowledge>();

                var subject = await subjectRepository.Find(new BaseSpecification<Subject>(s => s.Id == parameters.SubjectId));
                if (subject == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
                }

                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == parameters.KnowledgeId));
                if (knowledge == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                var subjectKnowledge = await subjectKnowledgeRepository.Find(
                    new BaseSpecification<SubjectKnowledge>(sk =>
                        sk.SubjectId == parameters.SubjectId && sk.KnowledgeId == parameters.KnowledgeId));

                if (subjectKnowledge == null)
                {
                    await subjectKnowledgeRepository.Add(new SubjectKnowledge
                    {
                        SubjectId = parameters.SubjectId,
                        KnowledgeId = parameters.KnowledgeId
                    });
                    return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Created);
                }

                await subjectKnowledgeRepository.Delete(subjectKnowledge);
                return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Deleted);
            }
            catch (Exception)
            {
                return Result<PivotSuccessModificationType>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
