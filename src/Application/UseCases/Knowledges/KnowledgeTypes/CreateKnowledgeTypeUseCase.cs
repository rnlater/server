using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class CreateKnowledgeTypeParams
    {
        public required string Name { get; set; }
        public Guid? ParentId { get; set; }
        public List<Guid> KnowledgeIds { get; set; } = [];
    }

    public class CreateKnowledgeTypeUseCase : IUseCase<KnowledgeTypeDto, CreateKnowledgeTypeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateKnowledgeTypeUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTypeDto>> Execute(CreateKnowledgeTypeParams parameters)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();

                var existingKnowledgeType = await knowledgeTypeRepository.Find(new BaseSpecification<KnowledgeType>(x => x.Name == parameters.Name));
                if (existingKnowledgeType != null)
                {
                    return Result<KnowledgeTypeDto>.Fail(ErrorMessage.KnowledgeTypeAlreadyExists);
                }

                var knowledgeType = new KnowledgeType
                {
                    Name = parameters.Name,
                };

                if (parameters.ParentId.HasValue)
                {
                    var parentKnowledgeType = await knowledgeTypeRepository.GetById(parameters.ParentId.Value);
                    if (parentKnowledgeType == null)
                    {
                        return Result<KnowledgeTypeDto>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                    }
                    knowledgeType.ParentId = parentKnowledgeType.Id;
                }

                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTypeKnowledgeRepository = _unitOfWork.Repository<KnowledgeTypeKnowledge>();

                knowledgeType = await knowledgeTypeRepository.Add(knowledgeType);

                foreach (var knowledgeId in parameters.KnowledgeIds)
                {
                    var knowledge = await knowledgeRepository.GetById(knowledgeId);
                    if (knowledge == null)
                    {
                        await _unitOfWork.RollBackChangesAsync();
                        return Result<KnowledgeTypeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                    }
                    await knowledgeTypeKnowledgeRepository.Add(
                        new KnowledgeTypeKnowledge
                        {
                            KnowledgeTypeId = knowledgeType.Id,
                            KnowledgeId = knowledge.Id
                        }
                    );
                }

                return Result<KnowledgeTypeDto>.Done(_mapper.Map<KnowledgeTypeDto>(knowledgeType));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<KnowledgeTypeDto>.Fail(ErrorMessage.UnknownError);
            }

        }
    }
}
