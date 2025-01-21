using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
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

namespace Application.UseCases.Knowledges.LearningLists
{
    public class GetLearningListByGuidUseCase : IUseCase<LearningListDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetLearningListByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<LearningListDto>> Execute(Guid guid)
        {
            try
            {
                var learningListRepository = _unitOfWork.Repository<LearningList>();
                var learningList = await learningListRepository.Find(
                    new BaseSpecification<LearningList>(ll => ll.Id == guid)
                    .AddInclude(query => query
                        .Include(ll => ll.User!)
                        .Include(ll => ll.LearningListKnowledges)
                        .ThenInclude(llk => llk.Knowledge!)));

                if (learningList == null)
                {
                    return Result<LearningListDto>.Fail(ErrorMessage.NoLearningListFoundWithGuid);
                }

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotFound);

                if (learningList.LearnerId != user.Id)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotAuthorized);

                var learningListKnowledgesToRemove = learningList.LearningListKnowledges
                    .Where(llk => llk.Knowledge!.Visibility == KnowledgeVisibility.Private && llk.Knowledge.CreatorId != user.Id)
                    .ToList();
                foreach (var llk in learningListKnowledgesToRemove)
                {
                    await _unitOfWork.Repository<LearningListKnowledge>().Delete(llk);
                    learningList.LearningListKnowledges.Remove(llk);
                }

                var learningListDto = _mapper.Map<LearningListDto>(learningList);

                var learningRepository = _unitOfWork.Repository<Learning>();
                foreach (var item in learningListDto.LearningListKnowledges)
                {
                    var learning = await learningRepository.Find(
                        new BaseSpecification<Learning>(l => l.UserId == userId && l.KnowledgeId == item.KnowledgeId));
                    var learningDto = _mapper.Map<LearningDto>(learning);
                    item.Knowledge!.CurrentUserLearning = learningDto;
                }

                return Result<LearningListDto>.Done(learningListDto);
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<LearningListDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}