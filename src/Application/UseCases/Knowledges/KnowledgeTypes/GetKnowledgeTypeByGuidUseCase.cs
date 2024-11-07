using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class GetKnowledgeTypeByGuidUseCase : IUseCase<KnowledgeTypeDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKnowledgeTypeByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTypeDto>> Execute(Guid id)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();
                var knowledgeType = await knowledgeTypeRepository.Find(
                    new BaseSpecification<KnowledgeType>(kt => kt.Id == id)
                    .AddInclude(query => query
                        .Include(kt => kt.Parent!)
                        .Include(kt => kt.Children)
                        .Include(kt => kt.KnowledgeTypeKnowledges)
                        .ThenInclude(ktk => ktk.Knowledge!)));

                if (knowledgeType == null)
                {
                    return Result<KnowledgeTypeDto>.Fail(ErrorMessage.NoKnowledgeTypeFoundWithGuid);
                }

                return Result<KnowledgeTypeDto>.Done(_mapper.Map<KnowledgeTypeDto>(knowledgeType));
            }
            catch (Exception)
            {
                return Result<KnowledgeTypeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
