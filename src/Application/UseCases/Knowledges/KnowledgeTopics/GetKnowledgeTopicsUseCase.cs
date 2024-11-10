using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class GetKnowledgeTopicsUseCase : IUseCase<IEnumerable<KnowledgeTopicDto>, NoParam>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKnowledgeTopicsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<KnowledgeTopicDto>>> Execute(NoParam parameters)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopics = await knowledgeTopicRepository.FindMany(new BaseSpecification<KnowledgeTopic>());

                if (!knowledgeTopics.Any())
                {
                    return Result<IEnumerable<KnowledgeTopicDto>>.Fail(ErrorMessage.NoKnowledgeTopicsFound);
                }

                return Result<IEnumerable<KnowledgeTopicDto>>.Done(knowledgeTopics.Select(_mapper.Map<KnowledgeTopicDto>));
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeTopicDto>>.Fail(ErrorMessage.UnknownError);
            }

        }
    }
}
