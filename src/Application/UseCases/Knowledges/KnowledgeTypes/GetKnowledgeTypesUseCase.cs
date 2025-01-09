using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;
using Application.Interfaces;

namespace Application.UseCases.Knowledges.KnowledgeTypes
{
    public class GetKnowledgeTypesParams
    {
        public string? Search { get; set; }
    }

    public class GetKnowledgeTypesUseCase : IUseCase<IEnumerable<KnowledgeTypeDto>, GetKnowledgeTypesParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCache _cache;


        public GetKnowledgeTypesUseCase(IUnitOfWork unitOfWork, IMapper mapper, IRedisCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<KnowledgeTypeDto>>> Execute(GetKnowledgeTypesParams parameters)
        {
            try
            {
                var knowledgeTypeRepository = _unitOfWork.Repository<KnowledgeType>();

                var knowledgeTypeDtos = await _cache.GetAsync<IEnumerable<KnowledgeTypeDto>>($"{RedisCache.Keys.GetKnowledgeTypes}");

                if (knowledgeTypeDtos == null)
                {
                    var knowledgeTypes = await knowledgeTypeRepository.FindMany(
                        new BaseSpecification<KnowledgeType>(kt => string.IsNullOrEmpty(parameters.Search) || kt.Name.Contains(parameters.Search)));

                    if (!knowledgeTypes.Any())
                    {
                        return Result<IEnumerable<KnowledgeTypeDto>>.Fail(ErrorMessage.NoKnowledgeTypesFound);
                    }

                    knowledgeTypeDtos = KnowledgeTypeDto.MergeArrangeKnowledgeTypes(_mapper.Map<IEnumerable<KnowledgeTypeDto>>(knowledgeTypes));
                    await _cache.SetAsync($"{RedisCache.Keys.GetKnowledgeTypes}", knowledgeTypeDtos);
                }

                return Result<IEnumerable<KnowledgeTypeDto>>.Done(knowledgeTypeDtos);
            }
            catch (Exception)
            {
                return Result<IEnumerable<KnowledgeTypeDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
