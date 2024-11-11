using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class AttachDetachKnowledgesParams
    {
        public Guid KnowledgeTypeId { get; set; }
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
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTypeKnowledgeRepository = _unitOfWork.Repository<KnowledgeTypeKnowledge>();

                var knowledgeType = await knowledgeTypeRepository.Find(new BaseSpecification<KnowledgeType>(kt => kt.Id == parameters.KnowledgeTypeId));
                if (knowledgeType == null)
                {
                    return Result<bool>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                }

                foreach (var knowledgeId in parameters.KnowledgeIds)
                {
                    var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == knowledgeId));
                    if (knowledge == null)
                    {
                        return Result<bool>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                    }

                    var knowledgeTypeKnowledge = await knowledgeTypeKnowledgeRepository.Find(
                        new BaseSpecification<KnowledgeTypeKnowledge>(ktk =>
                            ktk.KnowledgeTypeId == parameters.KnowledgeTypeId && ktk.KnowledgeId == knowledgeId));

                    if (knowledgeTypeKnowledge == null)
                    {
                        await knowledgeTypeKnowledgeRepository.Add(new KnowledgeTypeKnowledge
                        {
                            KnowledgeTypeId = parameters.KnowledgeTypeId,
                            KnowledgeId = knowledgeId
                        });
                    }
                    else
                    {
                        await knowledgeTypeKnowledgeRepository.Delete(knowledgeTypeKnowledge);
                    }
                }

                return Result<bool>.Done(true);
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                throw;
            }

        }
    }
}
