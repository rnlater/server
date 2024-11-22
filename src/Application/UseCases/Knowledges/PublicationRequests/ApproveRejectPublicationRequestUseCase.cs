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

namespace Application.UseCases.Knowledges.PublicationRequests
{
    public class ApproveRejectPublicationRequestParams
    {
        public Guid PublicationRequestId { get; set; }
        public bool IsApproved { get; set; }
    }
    public class ApproveRejectPublicationRequestUseCase : IUseCase<PublicationRequestDto, ApproveRejectPublicationRequestParams>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApproveRejectPublicationRequestUseCase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PublicationRequestDto>> Execute(ApproveRejectPublicationRequestParams parameters)
        {
            try
            {
                var publicationRequestRepository = _unitOfWork.Repository<PublicationRequest>();
                var publicationRequest = await publicationRequestRepository.Find(
                    new BaseSpecification<PublicationRequest>(pr => pr.Id == parameters.PublicationRequestId)
                    .AddInclude(query => query.Include(pr => pr.Knowledge!)));

                if (publicationRequest == null)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.NoPublicationRequestFoundWithGuid);
                else if (publicationRequest.Status != PublicationRequestStatus.Pending)
                    return Result<PublicationRequestDto>.Fail(ErrorMessage.PublicationRequestAlreadyApprovedOrRejected);

                publicationRequest.Status = parameters.IsApproved ? PublicationRequestStatus.Approved : PublicationRequestStatus.Rejected;
                if (parameters.IsApproved)
                {
                    publicationRequest.Knowledge!.Visibility = KnowledgeVisibility.Public;
                    publicationRequest.Knowledge = await _unitOfWork.Repository<Knowledge>().Update(publicationRequest.Knowledge);
                }

                publicationRequest = await publicationRequestRepository.Update(publicationRequest);

                return Result<PublicationRequestDto>.Done(_mapper.Map<PublicationRequestDto>(publicationRequest));
            }
            catch (Exception)
            {
                return Result<PublicationRequestDto>.Fail(ErrorMessage.UnknownError);
            }
        }
    }
}