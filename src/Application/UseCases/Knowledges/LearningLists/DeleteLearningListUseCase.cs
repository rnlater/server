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
    public class DeleteLearningListUseCase : IUseCase<LearningListDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteLearningListUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<LearningListDto>> Execute(Guid id)
        {
            try
            {
                var learningListRepository = _unitOfWork.Repository<LearningList>();
                var learningList = await learningListRepository.Find(new BaseSpecification<LearningList>(ll => ll.Id == id));

                if (learningList == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.NoLearningListFoundWithGuid);

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (userId == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotFound);

                if (learningList.LearnerId != userId.Value)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotAuthorized);

                learningList = await learningListRepository.Delete(id);

                return Result<LearningListDto>.Done(_mapper.Map<LearningListDto>(learningList));
            }
            catch (Exception)
            {
                return Result<LearningListDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}