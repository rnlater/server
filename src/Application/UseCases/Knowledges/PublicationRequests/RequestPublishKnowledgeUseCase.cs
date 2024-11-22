using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Types;
using Shared.Utils;

namespace Application.UseCases.Knowledges.PublicationRequests
{
    public class RequestPublishKnowledgeParams
    {
        public Guid KnowledgeId { get; set; }
    }
    public class RequestPublishKnowledgeUseCase : IUseCase<PublicationRequestDto, RequestPublishKnowledgeParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestPublishKnowledgeUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PublicationRequestDto>> Execute(RequestPublishKnowledgeParams parameters)
        {
            try
            {
                var userId = UserExtractor.GetUserId(_httpContextAccessor);
                var user = userId == null ? null : await _unitOfWork.Repository<User>().GetById(userId.Value);
                if (user == null)
                {
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.UserNotFound);
                }

                var knowledgeRepository = _unitOfWork.Repository<Knowledge>();
                var knowledge = await knowledgeRepository.Find(
                    new BaseSpecification<Knowledge>(k => k.Id == parameters.KnowledgeId)
                    .AddInclude(query => query.Include(k => k.PublicationRequest!)));

                if (knowledge == null)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                else if (knowledge.CreatorId != user.Id)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.UserNotAuthorized);
                else if (knowledge.PublicationRequest != null && knowledge.PublicationRequest.Status != PublicationRequestStatus.Rejected)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.KnowledgeAlreadyRequestedForPublication);

                var publicationRequestRepository = _unitOfWork.Repository<PublicationRequest>();
                var publicationRequest = new PublicationRequest
                {
                    KnowledgeId = parameters.KnowledgeId
                };

                publicationRequest = await publicationRequestRepository.Add(publicationRequest);

                return Result<PublicationRequestDto>.Done(_mapper.Map<PublicationRequestDto>(publicationRequest));
            }
            catch (Exception)
            {
                return Result<PublicationRequestDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}