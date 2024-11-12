using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges
{
    public class AttachDeattachKnowledgeTypeParams
    {
        public Guid KnowledgeId { get; set; }
        public Guid KnowledgeTypeId { get; set; }
    }

    public class AttachDeattachKnowledgeTypeUseCase : IUseCase<PivotSuccessModificationType, AttachDeattachKnowledgeTypeParams>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachDeattachKnowledgeTypeUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PivotSuccessModificationType>> Execute(AttachDeattachKnowledgeTypeParams parameters)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTypeKnowledgeRepository = _unitOfWork.Repository<KnowledgeTypeKnowledge>();

                var knowledgeType = await knowledgeTypeRepository.Find(new BaseSpecification<KnowledgeType>(kt => kt.Id == parameters.KnowledgeTypeId));
                if (knowledgeType == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                }

                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == parameters.KnowledgeId));
                if (knowledge == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                var knowledgeTypeKnowledge = await knowledgeTypeKnowledgeRepository.Find(new BaseSpecification<KnowledgeTypeKnowledge>(ktk => ktk.KnowledgeId == parameters.KnowledgeId && ktk.KnowledgeTypeId == parameters.KnowledgeTypeId));

                if (knowledgeTypeKnowledge == null)
                {
                    var newKnowledgeTypeKnowledge = new KnowledgeTypeKnowledge
                    {
                        KnowledgeId = parameters.KnowledgeId,
                        KnowledgeTypeId = parameters.KnowledgeTypeId
                    };

                    await knowledgeTypeKnowledgeRepository.Add(newKnowledgeTypeKnowledge);
                    return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Created);
                }
                else
                {
                    await knowledgeTypeKnowledgeRepository.Delete(knowledgeTypeKnowledge);
                    return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Deleted);
                }
            }
            catch (Exception)
            {
                return Result<PivotSuccessModificationType>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}