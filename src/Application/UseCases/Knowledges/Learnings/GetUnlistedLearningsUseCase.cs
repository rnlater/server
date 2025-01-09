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

namespace Application.UseCases.Knowledges.Learnings
{
    public class GetUnlistedLearningsUseCase : IUseCase<List<LearningDto>, NoParam>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUnlistedLearningsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<LearningDto>>> Execute(NoParam nothing)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);

                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<List<LearningDto>>.Fail(ErrorMessage.UserNotFound);

                var learningRepository = _unitOfWork.Repository<Learning>();
                var learningListRepository = _unitOfWork.Repository<LearningList>();
                IEnumerable<LearningList> LearntList = await learningListRepository.FindMany(
                               new BaseSpecification<LearningList>(ll => ll.LearnerId == userId)
                               .AddInclude(query => query.Include(ll => ll.LearningListKnowledges)));

                var learntLists = await learningListRepository.FindMany(
                    new BaseSpecification<LearningList>(ll => ll.LearnerId == userId)
                    .AddInclude(query => query.Include(ll => ll.LearningListKnowledges))
                );

                var learntKnowledgeIds = learntLists
                    .SelectMany(ll => ll.LearningListKnowledges)
                    .Select(llk => llk.KnowledgeId)
                    .Distinct()
                    .ToList();

                var unlistedLearnings = learntKnowledgeIds.Count != 0 ? await learningRepository.FindMany(
                    new BaseSpecification<Learning>(l => l.UserId == userId && !learntKnowledgeIds.Contains(l.KnowledgeId))
                    .AddInclude(query => query.Include(l => l.Knowledge!))
                ) : [];

                if (!unlistedLearnings.Any())
                {
                    return Result<List<LearningDto>>.Fail(ErrorMessage.NoLearningsFound);
                }

                var learningDtos = _mapper.Map<List<LearningDto>>(unlistedLearnings);

                return Result<List<LearningDto>>.Done(learningDtos);
            }
            catch (Exception)
            {
                return Result<List<LearningDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }

}