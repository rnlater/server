using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges
{

    public class PublishKnowledgeUseCase : IUseCase<KnowledgeDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublishKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeDto>> Execute(Guid guid)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == guid));

                if (knowledge == null)
                {
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }

                knowledge.Visibility = Domain.Enums.KnowledgeVisibility.Public;

                knowledge = await knowledgeRepository.Update(knowledge);

                return Result<KnowledgeDto>.Done(_mapper.Map<KnowledgeDto>(knowledge));
            }
            catch (Exception)
            {
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
