using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;
using Application.Interfaces;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class GetKnowledgeTopicsParams
    {
        public string? Search { get; set; }
    }
    public class GetKnowledgeTopicsUseCase : IUseCase<IEnumerable<KnowledgeTopicDto>, GetKnowledgeTopicsParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCache _cache;

        public GetKnowledgeTopicsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IRedisCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<KnowledgeTopicDto>>> Execute(GetKnowledgeTopicsParams parameters)
        {
            try
            {
                var knowledgeTopicRepository = _unitOfWork.Repository<KnowledgeTopic>();

                var knowledgeTopicDtos = await _cache.GetAsync<IEnumerable<KnowledgeTopicDto>>($"{RedisCache.Keys.GetKnowledgeTopics}");

                if (knowledgeTopicDtos == null)
                {
                    var knowledgeTopics = await knowledgeTopicRepository.FindMany(
                        new BaseSpecification<KnowledgeTopic>(
                            kt => string.IsNullOrEmpty(parameters.Search) || kt.Title.Contains(parameters.Search)
                        ));

                    if (!knowledgeTopics.Any())
                    {
                        return Result<IEnumerable<KnowledgeTopicDto>>.Fail(ErrorMessage.NoKnowledgeTopicsFound);
                    }

                    knowledgeTopicDtos = KnowledgeTopicDto.MergeArrangeKnowledgeTopics(_mapper.Map<IEnumerable<KnowledgeTopicDto>>(knowledgeTopics));
                    await _cache.SetAsync($"{RedisCache.Keys.GetKnowledgeTopics}", knowledgeTopicDtos);
                }

                return Result<IEnumerable<KnowledgeTopicDto>>.Done(knowledgeTopicDtos);
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeTopicDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
