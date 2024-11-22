using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.LearningLists
{

    public class AddRemoveKnowledgeToLearningListParams
    {
        public Guid LearningListId { get; set; }
        public Guid KnowledgeId { get; set; }
    }

    public class AddRemoveKnowledgeToLearningListUseCase : IUseCase<LearningListKnowledgeDto, AddRemoveKnowledgeToLearningListParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddRemoveKnowledgeToLearningListUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<LearningListKnowledgeDto>> Execute(AddRemoveKnowledgeToLearningListParams parameters)
        {
            try
            {
                var learningListKnowledgeRepository = _unitOfWork.Repository<LearningListKnowledge>();
                var existingLlk = await learningListKnowledgeRepository.Find(
                    new BaseSpecification<LearningListKnowledge>(llk =>
                        llk.LearningListId == parameters.LearningListId && llk.KnowledgeId == parameters.KnowledgeId)
                    .AddInclude(query => query
                        .Include(llk => llk.LearningList!)
                        .ThenInclude(ll => ll.User!)));

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                if (userId == null)
                    return Result<LearningListKnowledgeDto>.Fail(ErrorMessage.UserNotFound);

                if (existingLlk != null)
                {
                    if (existingLlk!.LearningList!.LearnerId != userId.Value)
                        return Result<LearningListKnowledgeDto>.Fail(ErrorMessage.UserNotAuthorized);

                    existingLlk = await learningListKnowledgeRepository.Delete(existingLlk);
                    return Result<LearningListKnowledgeDto>.Done(_mapper.Map<LearningListKnowledgeDto>(existingLlk));
                }

                var knowledge = await _unitOfWork.Repository<Knowledge>().GetById(parameters.KnowledgeId);

                if (knowledge!.Visibility == KnowledgeVisibility.Private
                    && knowledge!.CreatorId != userId.Value)
                    return Result<LearningListKnowledgeDto>.Fail(ErrorMessage.UserNotAuthorized);

                var learningListKnowledge = new LearningListKnowledge
                {
                    LearningListId = parameters.LearningListId,
                    KnowledgeId = parameters.KnowledgeId,
                };

                learningListKnowledge = await learningListKnowledgeRepository.Add(learningListKnowledge);

                return Result<LearningListKnowledgeDto>.Done(_mapper.Map<LearningListKnowledgeDto>(learningListKnowledge));
            }
            catch (Exception)
            {
                return Result<LearningListKnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}