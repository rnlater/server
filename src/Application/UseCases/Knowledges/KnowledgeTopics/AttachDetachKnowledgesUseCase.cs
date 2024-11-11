using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class AttachDetachKnowledgesParams
    {
        public Guid KnowledgeTopicId { get; set; }
        public List<Guid> KnowledgeIds { get; set; } = [];
    }

    public class AttachDetachKnowledgesUseCase : IUseCase<bool, AttachDetachKnowledgesParams>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachDetachKnowledgesUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Execute(AttachDetachKnowledgesParams parameters)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTopicKnowledgeRepository = _unitOfWork.Repository<KnowledgeTopicKnowledge>();

                var knowledgeTopic = await knowledgeTopicRepository.Find(new BaseSpecification<KnowledgeTopic>(kt => kt.Id == parameters.KnowledgeTopicId));
                if (knowledgeTopic == null)
                {
                    return Result<bool>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                }

                foreach (var knowledgeId in parameters.KnowledgeIds)
                {
                    var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == knowledgeId));
                    if (knowledge == null)
                    {
                        return Result<bool>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                    }

                    var knowledgeTopicKnowledge = await knowledgeTopicKnowledgeRepository.Find(
                        new BaseSpecification<KnowledgeTopicKnowledge>(ktk =>
                            ktk.KnowledgeTopicId == parameters.KnowledgeTopicId && ktk.KnowledgeId == knowledgeId));

                    if (knowledgeTopicKnowledge == null)
                    {
                        await knowledgeTopicKnowledgeRepository.Add(new KnowledgeTopicKnowledge
                        {
                            KnowledgeTopicId = parameters.KnowledgeTopicId,
                            KnowledgeId = knowledgeId
                        });
                    }
                    else
                    {
                        await knowledgeTopicKnowledgeRepository.Delete(knowledgeTopicKnowledge);
                    }
                }

                return Result<bool>.Done(true);
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<bool>.Fail(ErrorMessage.UnknownError);
            }

        }
    }
}
