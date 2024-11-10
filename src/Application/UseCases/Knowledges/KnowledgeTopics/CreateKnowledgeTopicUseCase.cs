using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Shared.Constants;
using Domain.Entities.PivotEntities;
using Domain.Base;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class CreateKnowledgeTopicParams
    {
        public required string Title { get; set; }
        public int? Order { get; set; }
        public Guid? ParentId { get; set; }
        public List<Guid> KnowledgeIds { get; set; } = [];
    }

    public class CreateKnowledgeTopicUseCase : IUseCase<KnowledgeTopicDto, CreateKnowledgeTopicParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateKnowledgeTopicUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<KnowledgeTopicDto>> Execute(CreateKnowledgeTopicParams parameters)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();
                var existingKnowledgeTopic = await knowledgeTopicRepository.Find(new BaseSpecification<KnowledgeTopic>(x => x.Title == parameters.Title));
                if (existingKnowledgeTopic != null)
                {
                    return Result<KnowledgeTopicDto>.Fail(ErrorMessage.KnowledgeTopicAlreadyExists);
                }

                var knowledgeTopic = new KnowledgeTopic
                {
                    Title = parameters.Title,
                };

                if (parameters.ParentId.HasValue)
                {
                    var parentKnowledgeTopic = await knowledgeTopicRepository.GetById(parameters.ParentId.Value);
                    if (parentKnowledgeTopic == null)
                    {
                        return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeTopicFoundWithGuid);
                    }
                    knowledgeTopic.ParentId = parentKnowledgeTopic.Id;
                }

                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledgeTopicKnowledgeRepository = _unitOfWork.Repository<KnowledgeTopicKnowledge>();

                knowledgeTopic = await knowledgeTopicRepository.Add(knowledgeTopic);

                foreach (var knowledgeId in parameters.KnowledgeIds)
                {
                    var knowledge = await knowledgeRepository.GetById(knowledgeId);
                    if (knowledge == null)
                    {
                        return Result<KnowledgeTopicDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                    }
                    await knowledgeTopicKnowledgeRepository.Add(
                        new KnowledgeTopicKnowledge
                        {
                            KnowledgeTopicId = knowledgeTopic.Id,
                            KnowledgeId = knowledge.Id
                        }
                    );
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
