using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class UpdateKnowledgeTypeParams
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
    public class UpdateKnowledgeTypeUseCase : IUseCase<KnowledgeTypeDto, UpdateKnowledgeTypeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateKnowledgeTypeUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTypeDto>> Execute(UpdateKnowledgeTypeParams parameters)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeType = await knowledgeTypeRepository.Find(new BaseSpecification<KnowledgeType>(kt => kt.Id == parameters.Id));

                if (knowledgeType == null)
                {
                    return Result<KnowledgeTypeDto>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                }

                knowledgeType.Name = parameters.Name;

                if (parameters.ParentId.HasValue)
                {
                    if (parameters.ParentId.Value == knowledgeType.Id)
                    {
                        return Result<KnowledgeTypeDto>.Fail(ErrorMessage.CannotBeParentOfItself);
                    }

                    var parentKnowledgeType = await knowledgeTypeRepository.Find(new BaseSpecification<KnowledgeType>(kt => kt.Id == parameters.ParentId.Value));
                    if (parentKnowledgeType == null)
                    {
                        return Result<KnowledgeTypeDto>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                    }
                    knowledgeType.ParentId = parentKnowledgeType.Id;
                }

                knowledgeType = await knowledgeTypeRepository.Update(knowledgeType);

                return Result<KnowledgeTypeDto>.Done(_mapper.Map<KnowledgeTypeDto>(knowledgeType));

            }
            catch (Exception)
            {
                return Result<KnowledgeTypeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
