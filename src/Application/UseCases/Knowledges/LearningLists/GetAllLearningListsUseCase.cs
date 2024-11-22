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
    public class GetAllLearningListsUseCase : IUseCase<IEnumerable<LearningListDto>, NoParam>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllLearningListsUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<IEnumerable<LearningListDto>>> Execute(NoParam nothing)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (userId == null)
                    return Result<IEnumerable<LearningListDto>>.Fail(ErrorMessage.UserNotFound);

                var learningListRepository = _unitOfWork.Repository<LearningList>();
                var learningLists = await learningListRepository.FindMany(
                    new BaseSpecification<LearningList>(ll => ll.LearnerId == userId));

                return Result<IEnumerable<LearningListDto>>.Done(_mapper.Map<IEnumerable<LearningListDto>>(learningLists));
            }
            catch (Exception)
            {
                return Result<IEnumerable<LearningListDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}