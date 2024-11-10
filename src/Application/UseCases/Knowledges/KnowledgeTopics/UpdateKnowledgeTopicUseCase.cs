using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class UpdateKnowledgeTopicParams
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int? Order { get; set; }
        public Guid? ParentId { get; set; }
    }
    public class UpdateKnowledgeTopicUseCase : IUseCase<KnowledgeTopicDto, UpdateKnowledgeTopicParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateKnowledgeTopicUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTopicDto>> Execute(UpdateKnowledgeTopicParams parameters)
        {
            try
            {

                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var knowledgeTopic = await knowledgeTopicRepository.Find(new BaseSpecification<KnowledgeTopic>(kt => kt.Id == parameters.Id));

                if (knowledgeTopic == null)
                {
                    return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                }

                knowledgeTopic.Title = parameters.Title;
                knowledgeTopic.Order = parameters.Order;

                if (parameters.ParentId.HasValue)
                {
                    if (parameters.ParentId.Value == knowledgeTopic.Id)
                    {
                        return Result<KnowledgeTopicDto>.Fail(ErrorMessage.CannotBeParentOfItself);
                    }

                    var parentKnowledgeType = await knowledgeTopicRepository.GetById(parameters.ParentId.Value);
                    if (parentKnowledgeType == null)
                    {
                        return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                    }
                    knowledgeTopic.ParentId = parentKnowledgeType.Id;
                }

                knowledgeTopic = await knowledgeTopicRepository.Update(knowledgeTopic);

                return Result<KnowledgeTopicDto>.Done(_mapper.Map<KnowledgeTopicDto>(knowledgeTopic));
            }
            catch (Exception)
            {
                return Result<KnowledgeTopicDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
