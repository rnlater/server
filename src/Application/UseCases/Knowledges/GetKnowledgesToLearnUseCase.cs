using Application.DTOs;
using AutoMapper;
using Domain.Base;
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
}

public class KnowledgeDataToLearn
{
    public required KnowledgeDto Knowledge { get; set; }
    public Guid CorrectGameOptionId { get; set; }
    public required string Interpretation { get; set; }
}

public class GetKnowledgesToLearnUseCase : IUseCase<List<Dictionary<Guid, KnowledgeDataToLearn>>, GetKnowledgesToLearnParams>
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
    public async Task<Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>> Execute(GetKnowledgesToLearnParams parameters)
    {
        try
        {
            var userId = UserExtractor.GetUserId(_httpContextAccessor);
            if (userId == null)
                return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Fail(ErrorMessage.UserNotFound);

            var learningsCount = await _unitOfWork.Repository<Learning>().Count(new BaseSpecification<Learning>(
                l => l.UserId == userId && parameters.KnowledgeIds.Contains(l.KnowledgeId)
            ));
            if (learningsCount != 0)
                return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Fail(ErrorMessage.KnowledgeAlreadyLearned);

            var knowledges = await _unitOfWork.Repository<Knowledge>().FindMany(
                new BaseSpecification<Knowledge>(
                    k => parameters.KnowledgeIds.Contains(k.Id)
                ).AddInclude(query => query
                    .Include(k => k.Materials)
                    .Include(k => k.GameKnowledgeSubscriptions)
                    .ThenInclude(gk => gk.Game!)
                    .Include(k => k.GameKnowledgeSubscriptions)
                    .ThenInclude(gk => gk.GameOptions))
            );
            if (knowledges.Count() != parameters.KnowledgeIds.Count)
                return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Fail(ErrorMessage.SomeKnowledgesNotFound);

            List<List<Knowledge>> knowledgeGroups = Randomer.GetRandomGroups(knowledges.ToList());

            List<Dictionary<Guid, KnowledgeDataToLearn>> knowledgeDataToLearnResponses = [];

            for (int i = 0; i < knowledgeGroups.Count; i++)
            {
                var _knowledgeDtos = _mapper.Map<List<KnowledgeDto>>(knowledgeGroups[i]);
                Dictionary<Guid, KnowledgeDataToLearn> knowledgeDataToLearn = [];

                foreach (KnowledgeDto? knowledge in _knowledgeDtos)
                {
                    string DistinctInterpretationMaterial = knowledge.Materials
                        .Where(m => m.Type == MaterialType.Interpretation.ToString())
                        .Select(m => m.Content)
                        .OrderBy(_ => new Random().Next())
                        .First();

                    var subscriptions = knowledge.GameKnowledgeSubscriptions.ToList();
                    if (knowledge.GameKnowledgeSubscriptions.Count < 1)
                        return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Fail(ErrorMessage.RequireAGameToReview);

                    if (knowledge.GameKnowledgeSubscriptions.Count > 1)
                        knowledge.GameKnowledgeSubscriptions = Randomer.GetRandomElementAsList(knowledge.GameKnowledgeSubscriptions.ToList());

                    knowledge.GameToReview = knowledge.GameKnowledgeSubscriptions.First().DistinctGroupedGameOptions();
                    knowledge.GameKnowledgeSubscriptions = [];

                    knowledgeDataToLearn.Add(knowledge.Id, new KnowledgeDataToLearn
                    {
                        Knowledge = knowledge,
                        CorrectGameOptionId = knowledge.GameToReview.GetCorrectGameOption().Id,
                        Interpretation = DistinctInterpretationMaterial
                    });
                }

                knowledgeDataToLearnResponses.Add(knowledgeDataToLearn);
            }

            return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Done(knowledgeDataToLearnResponses);
        }
        catch (Exception)
        {
            return Result<List<Dictionary<Guid, KnowledgeDataToLearn>>>.Fail(ErrorMessage.UnknownError);
        }
    }

    public static List<List<Knowledge>> GetRandomKnowledgeGroups(List<Knowledge> knowledges, int maxGroupSize = 6, int minGroupSize = 4)
    {
        var random = new Random();
        var knowledgeGroups = new List<List<Knowledge>>();
        var shuffledKnowledges = knowledges.OrderBy(_ => random.Next()).ToList();

        if (shuffledKnowledges.Count < minGroupSize)
        {
            knowledgeGroups.Add(shuffledKnowledges);
            return knowledgeGroups;
        }

        int i = 0;
        while (i < shuffledKnowledges.Count)
        {
            int remaining = shuffledKnowledges.Count - i;

            int groupSize = random.Next(minGroupSize, Math.Min(maxGroupSize, remaining) + 1);

            var group = shuffledKnowledges.Skip(i).Take(groupSize).ToList();
            knowledgeGroups.Add(group);

            i += groupSize;
        }

        if (knowledgeGroups.Count > 1 && knowledgeGroups.Last().Count < minGroupSize)
        {
            var lastGroup = knowledgeGroups.Last();
            knowledgeGroups.RemoveAt(knowledgeGroups.Count - 1);

            foreach (var knowledge in lastGroup)
            {
                knowledgeGroups[^1].Add(knowledge);
            }
        }

        return knowledgeGroups;
    }

}
