using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges
{
    public class AttachDeattachKnowledgeTopicParams
    {
        public Guid KnowledgeId { get; set; }
        public Guid KnowledgeTopicId { get; set; }
    }

    public class AttachDeattachKnowledgeTopicUseCase : IUseCase<PivotSuccessModificationType, AttachDeattachKnowledgeTopicParams>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachDeattachKnowledgeTopicUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PivotSuccessModificationType>> Execute(AttachDeattachKnowledgeTopicParams parameters)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTopicKnowledgeRepository = _unitOfWork.Repository<KnowledgeTopicKnowledge>();

                var knowledgeTopic = await knowledgeTopicRepository.Find(new BaseSpecification<KnowledgeTopic>(kt => kt.Id == parameters.KnowledgeTopicId));
                if (knowledgeTopic == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                }

                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == parameters.KnowledgeId));
                if (knowledge == null)
                {
                    return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                var knowledgeTopicKnowledge = await knowledgeTopicKnowledgeRepository.Find(new BaseSpecification<KnowledgeTopicKnowledge>(ktk => ktk.KnowledgeId == parameters.KnowledgeId && ktk.KnowledgeTopicId == parameters.KnowledgeTopicId));

                if (knowledgeTopicKnowledge == null)
                {
                    var newKnowledgeTopicKnowledge = new KnowledgeTopicKnowledge
                    {
                        KnowledgeId = parameters.KnowledgeId,
                        KnowledgeTopicId = parameters.KnowledgeTopicId
                    };

                    await knowledgeTopicKnowledgeRepository.Add(newKnowledgeTopicKnowledge);
                    return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Created);
                }
                else
                {
                    await knowledgeTopicKnowledgeRepository.Delete(knowledgeTopicKnowledge);
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