using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.LearningLists
{
    public class UpdateLearningListParams
    {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
    }

    public class UpdateLearningListUseCase : IUseCase<LearningListDto, UpdateLearningListParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateLearningListUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<LearningListDto>> Execute(UpdateLearningListParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (userId == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotFound);

                var learningListRepository = _unitOfWork.Repository<LearningList>();

                var learningList = await learningListRepository.GetById(parameters.Id);

                if (learningList == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.NoLearningListFoundWithGuid);
                else if (learningList.LearnerId != userId)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotAuthorized);

                var IsTitleExisted = await learningListRepository.Find(new BaseSpecification<LearningList>(ll => ll.Title == parameters.Title && ll.LearnerId == userId)) != null;
                if (IsTitleExisted)
                    return Result<LearningListDto>.Fail(ErrorMessage.LearningListTitleExisted);

                learningList = await learningListRepository.Update(learningList);

                return Result<LearningListDto>.Done(_mapper.Map<LearningListDto>(learningList));
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackChangesAsync();
                return Result<LearningListDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}