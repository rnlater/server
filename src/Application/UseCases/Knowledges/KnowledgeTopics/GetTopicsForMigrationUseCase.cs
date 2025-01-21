using Application.DTOs;
using AutoMapper;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Types;
using Domain.Base;
using Shared.Constants;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.SingleIdPivotEntities;
using Shared.Utils;
using Microsoft.AspNetCore.Http;
using Application.DTOs.SingleIdPivotEntities;
using Domain.Enums;

namespace Application.UseCases.Knowledges.KnowledgeTopics
{
    public class GetTopicsForMigrationParams
    {
        public Guid? ParentId { get; set; }
    }

    public class GetTopicsForMigrationUseCase : IUseCase<IEnumerable<KnowledgeTopicDto>, GetTopicsForMigrationParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRedisCache _cache;

        public GetTopicsForMigrationUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRedisCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
        }

        public async Task<Result<IEnumerable<KnowledgeTopicDto>>> Execute(GetTopicsForMigrationParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);

                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<IEnumerable<KnowledgeTopicDto>>.Fail(ErrorMessage.UserNotFound);

                var cacheKey = RedisCache.Keys.GetKnowledgeTopicsForMigration(parameters.ParentId);
                var knowledgeTopicDtos = await _cache.GetAsync<IEnumerable<KnowledgeTopicDto>>(cacheKey);

                if (knowledgeTopicDtos == null)
                {
                    var specification = new BaseSpecification<KnowledgeTopic>(x => x.ParentId == parameters.ParentId);
                    specification.AddInclude(query => query.Include(kt => kt.Children)
                                           .Include(kt => kt.KnowledgeTopicKnowledges)
                                           .ThenInclude(ktk => ktk.Knowledge!));

                    var knowledgeTopics = await _unitOfWork.Repository<KnowledgeTopic>().FindMany(specification);

                    foreach (var item in knowledgeTopics)
                    {
                        item.KnowledgeTopicKnowledges = [.. item.KnowledgeTopicKnowledges.Where(ktk => ktk.Knowledge!.Visibility == KnowledgeVisibility.Public)];
                    }
                    knowledgeTopicDtos = _mapper.Map<IEnumerable<KnowledgeTopicDto>>(knowledgeTopics);

                    await _cache.SetAsync(cacheKey, knowledgeTopicDtos);
                }

                foreach (var item in knowledgeTopicDtos)
                {
                    foreach (var knowledge in item.KnowledgeTopicKnowledges.Select(ktk => ktk.Knowledge))
                    {
                        var learningSpec = new BaseSpecification<Learning>(x => x.KnowledgeId == knowledge!.Id && x.UserId == user.Id);
                        var learning = await _unitOfWork.Repository<Learning>().Find(learningSpec);
                        knowledge!.CurrentUserLearning = learning == null ? null : _mapper.Map<LearningDto>(learning);
                    }
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
