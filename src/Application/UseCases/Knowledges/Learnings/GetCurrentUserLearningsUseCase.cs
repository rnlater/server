using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Entities.SingleIdPivotEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.Learnings;

public class GetCurrentUserLearningsParams
{
    public string? Search { get; set; }
}

public class CurrentUserLearnings
{
    public List<LearningListDto> LearntList { get; set; } = [];
    public List<LearningDto> UnlistedLearnings { get; set; } = [];
}

public class GetCurrentUserLearningsUseCase : IUseCase<CurrentUserLearnings, GetCurrentUserLearningsParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserLearningsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<CurrentUserLearnings>> Execute(GetCurrentUserLearningsParams parameters)
    {
        try
        {
            var userId = UserExtractor.GetUserId(_httpContextAccessor);

            var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
            if (userId == null)
                return Result<CurrentUserLearnings>.Fail(ErrorMessage.UserNotFound);

            var learningRepository = _unitOfWork.Repository<Learning>();
            var learningListRepository = _unitOfWork.Repository<LearningList>();

            var LearntList = await learningListRepository.FindMany(
                new BaseSpecification<LearningList>(ll =>
                    ll.LearnerId == userId
                    && (parameters.Search == null
                            || ll.Title.Contains(parameters.Search)
                            || ll.LearningListKnowledges.Any(llk => llk.Knowledge!.Title.Contains(parameters.Search))))
                .AddInclude(query => query
                    .Include(ll => ll.LearningListKnowledges)
                    .ThenInclude(llk => llk.Knowledge!)
            ));
            var LearntListDto = _mapper.Map<List<LearningListDto>>(LearntList);
            for (int i = 0; i < LearntList.Count(); i++)
            {
                ICollection<LearningDto> LearntKnowledges = [];
                ICollection<KnowledgeDto> NotLearntKnowledges = [];
                foreach (var item in LearntListDto[i].LearningListKnowledges)
                {
                    var learning = await learningRepository.Find(
                        new BaseSpecification<Learning>(l => l.UserId == userId && l.KnowledgeId == item.KnowledgeId));
                    if (learning == null)
                        NotLearntKnowledges.Add(item.Knowledge!);
                    else
                    {
                        var learningDto = _mapper.Map<LearningDto>(learning);
                        learningDto.Knowledge = item.Knowledge;
                        LearntKnowledges.Add(learningDto);
                    }
                }

                LearntListDto[i].LearntKnowledges = LearntKnowledges;
                LearntListDto[i].NotLearntKnowledges = NotLearntKnowledges;
                LearntListDto[i].LearningListKnowledges = [];
            }

            var UnlistedLearnings = await learningRepository.FindMany(
                new BaseSpecification<Learning>(ll =>
                    ll.UserId == userId
                    && !LearntList.SelectMany(ll => ll.LearningListKnowledges.Select(llk => llk.KnowledgeId)).Distinct().Contains(ll.KnowledgeId)
                    && (parameters.Search == null
                            || ll.Knowledge!.Title.Contains(parameters.Search))
                )
                .AddInclude(query => query
                    .Include(ll => ll.Knowledge!)
            ));
            var UnlistedLearningsDto = _mapper.Map<List<LearningDto>>(UnlistedLearnings);

            if (parameters.Search != null && UnlistedLearningsDto.Count() == 0 && LearntListDto.Count() == 0)
            {
                return Result<CurrentUserLearnings>.Fail(ErrorMessage.NoData);
            }
            return Result<CurrentUserLearnings>.Done(new CurrentUserLearnings
            {
                LearntList = LearntListDto ?? [],
                UnlistedLearnings = UnlistedLearningsDto ?? []
            });
        }
        catch (Exception)
        {
            return Result<CurrentUserLearnings>.Fail(ErrorMessage.UnknownError);
        }
    }
}
