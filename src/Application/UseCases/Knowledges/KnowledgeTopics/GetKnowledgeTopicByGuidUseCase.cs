using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class GetKnowledgeTopicByGuidUseCase : IUseCase<KnowledgeTopicDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKnowledgeTopicByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTopicDto>> Execute(Guid id)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopic = await knowledgeTopicRepository.Find(
                    new BaseSpecification<KnowledgeTopic>(kt => kt.Id == id)
                    .AddInclude(query => query
                        .Include(kt => kt.KnowledgeTopicKnowledges)
                        .ThenInclude(ktk => ktk.Knowledge!)));

                if (knowledgeTopic == null)
                {
                    return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                }

                return Result<KnowledgeTopicDto>.Done(_mapper.Map<KnowledgeTopicDto>(knowledgeTopic));
            }
            catch (Exception)
            {
                return Result<KnowledgeTopicDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
