using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class DeleteKnowledgeTopicUseCase : IUseCase<KnowledgeTopicDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteKnowledgeTopicUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTopicDto>> Execute(Guid id)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopic = await knowledgeTopicRepository.Find(new BaseSpecification<KnowledgeTopic>(kt => kt.Id == id));

                if (knowledgeTopic == null)
                {
                    return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                }

                knowledgeTopic = await knowledgeTopicRepository.Delete(id);

                return Result<KnowledgeTopicDto>.Done(_mapper.Map<KnowledgeTopicDto>(knowledgeTopic));
            }
            catch (Exception)
            {
                return Result<KnowledgeTopicDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
