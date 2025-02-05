using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges;

public class GetKnowledgesToLearnParams
{
    public List<Guid> KnowledgeIds { get; set; } = [];
    public string? NewLearningListTitle { get; set; }
}

public class KnowledgeDataToLearn
{
    public required KnowledgeDto Knowledge { get; set; }
    public required string Interpretation { get; set; }
}

public class GetKnowledgesToLearnUseCase : IUseCase<List<List<KnowledgeDto>>, GetKnowledgesToLearnParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetKnowledgesToLearnUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Result<List<List<KnowledgeDto>>>> Execute(GetKnowledgesToLearnParams parameters)
    {
        try
        {
            var userId = UserExtractor.GetUserId(_httpContextAccessor);
            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (user == null)
                return Result<List<List<KnowledgeDto>>>.Fail(ErrorMessage.UserNotFound);

            var learningsCount = await _unitOfWork.Repository<Learning>().Count(new BaseSpecification<Learning>(
                l => l.UserId == userId && parameters.KnowledgeIds.Contains(l.KnowledgeId)
            ));
            if (learningsCount != 0)
                return Result<List<List<KnowledgeDto>>>.Fail(ErrorMessage.KnowledgeAlreadyLearned);

            var knowledges = await _unitOfWork.Repository<Knowledge>().FindMany(
                new BaseSpecification<Knowledge>(
                    k => parameters.KnowledgeIds.Contains(k.Id) && (k.Visibility == KnowledgeVisibility.Public || (k.Visibility == KnowledgeVisibility.Private && k.CreatorId == userId))
                ).AddInclude(query => query
                    .Include(k => k.Materials)
                    .Include(k => k.GameKnowledgeSubscriptions)
                    .ThenInclude(gk => gk.Game!)
                    .Include(k => k.GameKnowledgeSubscriptions)
                    .ThenInclude(gk => gk.GameOptions))
            );
            if (knowledges.Count() != parameters.KnowledgeIds.Count)
                return Result<List<List<KnowledgeDto>>>.Fail(ErrorMessage.SomeKnowledgesNotFound);

            List<List<KnowledgeDto>> knowledgeDataToLearnResponses = [];

            List<List<Knowledge>> knowledgeGroups = Randomer.GetRandomGroups(knowledges.ToList());
            for (int i = 0; i < knowledgeGroups.Count; i++)
            {
                var _knowledgeDtos = _mapper.Map<List<KnowledgeDto>>(knowledgeGroups[i]);
                foreach (KnowledgeDto? knowledge in _knowledgeDtos)
                {
                    if (knowledge.GameKnowledgeSubscriptions.Count < 2)
                        return Result<List<List<KnowledgeDto>>>.Fail(ErrorMessage.RequireTwoGamesToLearn);

                    if (knowledge.GameKnowledgeSubscriptions.Count > 2)
                        knowledge.GameKnowledgeSubscriptions = Randomer.GetRandomElementsAsList(knowledge.GameKnowledgeSubscriptions.ToList(), 2);

                    knowledge.GamesToLearn = knowledge.GameKnowledgeSubscriptions.Select(gks => gks.DistinctGroupedGameOptions()).ToArray();
                    knowledge.GameKnowledgeSubscriptions = [];
                }

                knowledgeDataToLearnResponses.Add(_knowledgeDtos);
            }

            if (parameters.NewLearningListTitle != null)
            {
                var learningList = await _unitOfWork.Repository<LearningList>().Find(new BaseSpecification<LearningList>(
                    ll => ll.Title == parameters.NewLearningListTitle && ll.LearnerId == userId
                ));
                learningList ??= await _unitOfWork.Repository<LearningList>().Add(new LearningList
                {
                    Title = parameters.NewLearningListTitle,
                    LearnerId = user.Id
                });

                foreach (var knowledge in knowledges)
                {
                    if (await _unitOfWork.Repository<LearningListKnowledge>().Find(
                        new BaseSpecification<LearningListKnowledge>(
                            llk => llk.LearningListId == learningList.Id && llk.KnowledgeId == knowledge.Id
                        )) != null) continue;
                    await _unitOfWork.Repository<LearningListKnowledge>().Add(new LearningListKnowledge
                    {
                        KnowledgeId = knowledge.Id,
                        LearningListId = learningList.Id
                    });
                }
            }

            return Result<List<List<KnowledgeDto>>>.Done(knowledgeDataToLearnResponses);
        }
        catch (Exception)
        {
            return Result<List<List<KnowledgeDto>>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
