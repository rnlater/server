using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
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

namespace Application.UseCases.Knowledges
{
    public class GetDetailedKnowledgeByGuidUseCase : IUseCase<KnowledgeDto, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDetailedKnowledgeByGuidUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<KnowledgeDto>> Execute(Guid id)
        {
            try
            {
                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(
                    new BaseSpecification<Knowledge>(k => k.Id == id)
                    .AddInclude(query => query
                        .Include(k => k.KnowledgeTypeKnowledges)
                        .ThenInclude(kt => kt.KnowledgeType!)
                        .Include(k => k.KnowledgeTopicKnowledges)
                        .ThenInclude(kt => kt.KnowledgeTopic!)
                        .Include(k => k.SubjectKnowledges)
                        .ThenInclude(kt => kt.Subject!)
                        .Include(k => k.Creator)
                        .Include(k => k.Materials)
                ));

                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                    return Result<KnowledgeDto>.Fail(ErrorMessage.UserNotFound);

                if (knowledge == null || (!user.IsAdmin && knowledge.CreatorId != userId && knowledge.Visibility == KnowledgeVisibility.Private))
                    return Result<KnowledgeDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);

                var KnowledgeDto = _mapper.Map<KnowledgeDto>(knowledge);

                if (!user.IsAdmin)
                {
                    var userLearning = await _unitOfWork.Repository<Learning>().Find(
                        new BaseSpecification<Learning>(ul => ul.UserId == userId && ul.KnowledgeId == id)
                    );
                    KnowledgeDto.CurrentUserLearning = userLearning == null ? null : _mapper.Map<LearningDto>(userLearning);
                }

                return Result<KnowledgeDto>.Done(KnowledgeDto);
            }
            catch (Exception)
            {
                return Result<KnowledgeDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}
