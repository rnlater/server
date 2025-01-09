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
    public class AddRemoveKnowledgesToLearningListParams
    {
        public Guid LearningListId { get; set; }
        public required List<Guid> KnowledgeIds { get; set; } = [];
        public required bool IsAdd { get; set; }
    }

    public class AddRemoveKnowledgesToLearningListUseCase : IUseCase<List<LearningListKnowledgeDto>, AddRemoveKnowledgesToLearningListParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddRemoveKnowledgesToLearningListUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<LearningListKnowledgeDto>>> Execute(AddRemoveKnowledgesToLearningListParams parameters)
        {
            try
            {
                var result = new List<LearningListKnowledgeDto>();
                var learningListKnowledgeRepository = _unitOfWork.Repository<LearningListKnowledge>();
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<List<LearningListKnowledgeDto>>.Fail(ErrorMessage.UserNotFound);

                foreach (var knowledgeId in parameters.KnowledgeIds)
                {
                    var existingLlk = await learningListKnowledgeRepository.Find(
                        new BaseSpecification<LearningListKnowledge>(llk =>
                            llk.LearningListId == parameters.LearningListId && llk.KnowledgeId == knowledgeId)
                        .AddInclude(query => query.Include(llk => llk.LearningList!)));

                    var knowledge = await _unitOfWork.Repository<Knowledge>().GetById(knowledgeId);
                    if (knowledge == null)
                        return Result<List<LearningListKnowledgeDto>>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);

                    if (knowledge.Visibility == KnowledgeVisibility.Private && knowledge.CreatorId != user.Id)
                        return Result<List<LearningListKnowledgeDto>>.Fail(ErrorMessage.UserNotAuthorized);

                    var learningDto = _mapper.Map<LearningDto>(await _unitOfWork.Repository<Learning>().Find(
                        new BaseSpecification<Learning>(l => l.UserId == userId && l.KnowledgeId == knowledge.Id)));
                    var knowledgeDto = _mapper.Map<KnowledgeDto>(knowledge);
                    knowledgeDto!.CurrentUserLearning = learningDto;

                    if (parameters.IsAdd)
                    {
                        if (existingLlk != null)
                            continue;

                        var learningListKnowledge = await learningListKnowledgeRepository.Add(new LearningListKnowledge
                        {
                            LearningListId = parameters.LearningListId,
                            KnowledgeId = knowledgeId,
                        });
                        var learningListKnowledgeDto = _mapper.Map<LearningListKnowledgeDto>(learningListKnowledge);
                        learningListKnowledgeDto.Knowledge = knowledgeDto;
                        result.Add(learningListKnowledgeDto);
                    }
                    else if (existingLlk != null)
                    {
                        if (existingLlk.LearningList!.LearnerId != user.Id)
                            return Result<List<LearningListKnowledgeDto>>.Fail(ErrorMessage.UserNotAuthorized);

                        existingLlk.LearningList = null;
                        existingLlk = await learningListKnowledgeRepository.Delete(existingLlk);
                        var learningListKnowledgeDto = _mapper.Map<LearningListKnowledgeDto>(existingLlk);
                        learningListKnowledgeDto.Knowledge = knowledgeDto;
                        result.Add(learningListKnowledgeDto);
                    }

                }

                return Result<List<LearningListKnowledgeDto>>.Done(result);
            }
            catch (Exception)
            {
                return Result<List<LearningListKnowledgeDto>>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}