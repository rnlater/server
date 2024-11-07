using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class GetKnowledgeTypesUseCase : IUseCase<IEnumerable<KnowledgeTypeDto>, NoParam>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKnowledgeTypesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<KnowledgeTypeDto>>> Execute(NoParam parameters)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeTypes = await knowledgeTypeRepository.FindMany(new BaseSpecification<KnowledgeType>());

                if (!knowledgeTypes.Any())
                {
                    return Result<IEnumerable<KnowledgeTypeDto>>.Fail(ErrorMessage.NoKnowledgeTypesFound);
                }

                return Result<IEnumerable<KnowledgeTypeDto>>.Done(knowledgeTypes.Select(_mapper.Map<KnowledgeTypeDto>));
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeTypeDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
