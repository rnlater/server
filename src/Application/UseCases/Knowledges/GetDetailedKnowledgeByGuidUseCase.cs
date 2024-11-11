using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges
{
    public class GetDetailedKnowledgeByGuidUseCase : IUseCase<KnowledgeDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDetailedKnowledgeByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeDto>> Execute(Guid id)
        {
            try
            {

                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(
                    new BaseSpecification<Knowledge>(k => k.Id == id)
                    .AddInclude(query => query
                        .Include(k => k.KnowledgeTypeKnowledges)
                        .ThenInclude(kt => kt.KnowledgeType!)
                        .Include(k => k.KnowledgeTopicKnowledges)
                        .ThenInclude(kt => kt.KnowledgeTopic!)
                        .Include(k => k.SubjectKnowledges)
                        .ThenInclude(kt => kt.Subject!)
                        .Include(k => k.Creator)
                        .Include(k => k.Materials)
                ));

                if (knowledge == null)
                {
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                return Result<KnowledgeDto>.Done(_mapper.Map<KnowledgeDto>(knowledge));
            }
            catch (Exception)
            {
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
