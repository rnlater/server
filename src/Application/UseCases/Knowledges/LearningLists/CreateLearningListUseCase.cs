using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.LearningLists
{
    public class CreateLearningListParams
    {
        public required string Title { get; set; }
        public List<Guid>? KnowledgeIds { get; set; }
    }

    public class CreateLearningListUseCase : IUseCase<LearningListDto, CreateLearningListParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CreateLearningListUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<LearningListDto>> Execute(CreateLearningListParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<LearningListDto>.Fail(ErrorMessage.UserNotFound);

                var learningListRepository = _unitOfWork.Repository<LearningList>();
                var IsExisted = await learningListRepository.Find(new BaseSpecification<LearningList>(ll => ll.Title == parameters.Title && ll.LearnerId == userId)) != null;
                if (IsExisted)
                    return Result<LearningListDto>.Fail(ErrorMessage.LearningListTitleExisted);

                var learningList = new LearningList
                {
                    Title = parameters.Title,
                    LearnerId = user.Id
                };

                learningList = await learningListRepository.Add(learningList);
                var learningListDto = _mapper.Map<LearningListDto>(learningList);

                if (parameters.KnowledgeIds != null)
                {
                    var knowledges = await _unitOfWork.Repository<Knowledge>().FindMany(
                        new BaseSpecification<Knowledge>(k => parameters.KnowledgeIds.Contains(k.Id))
                    );

                    if (knowledges == null || knowledges.Count() != parameters.KnowledgeIds.Count)
                    {
                        await _unitOfWork.RollBackChangesAsync();
                        return Result<LearningListDto>.Fail(ErrorMessage.SomeKnowledgesNotFound);
                    }
                    else if (knowledges.Any(k => k.Visibility == KnowledgeVisibility.Private && k.CreatorId != userId))
                    {
                        await _unitOfWork.RollBackChangesAsync();
                        return Result<LearningListDto>.Fail(ErrorMessage.UserNotAuthorized);
                    }

                    foreach (var knowledge in knowledges)
                    {
                        var learningListKnowledge = new LearningListKnowledge
                        {
                            LearningListId = learningList.Id,
                            KnowledgeId = knowledge.Id
                        };

                        await _unitOfWork.Repository<LearningListKnowledge>().Add(learningListKnowledge);
                    }
                    learningListDto.NotLearntKnowledgeCount = 0;
                    learningListDto.LearntKnowledgeCount = knowledges.Count();
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