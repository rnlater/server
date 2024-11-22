using Application.DTOs;
using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.PublicationRequests;
using Shared.Types;

namespace Application.Services.Knowledges
{
    public class PublicationRequestService : IPublicationRequestService
    {
        private readonly RequestPublishKnowledgeUseCase _requestPublishKnowledgeUseCase;
        private readonly DeletePublicationRequestUseCase _deletePublicationRequestUseCase;
        private readonly GetPublicationRequestsUseCase _getPublicationRequestsUseCase;
        private readonly ApproveRejectPublicationRequestUseCase _approveRejectPublicationRequestUseCase;
        private readonly UpdateKnowledgeVisibilityUseCase _updateKnowledgeVisibilityUseCase;

        public PublicationRequestService(
            RequestPublishKnowledgeUseCase requestPublishKnowledgeUseCase,
            DeletePublicationRequestUseCase deletePublicationRequestUseCase,
            GetPublicationRequestsUseCase getPublicationRequestsUseCase,
            ApproveRejectPublicationRequestUseCase approveRejectPublicationRequestUseCase,
            UpdateKnowledgeVisibilityUseCase updateKnowledgeVisibilityUseCase)
        {
            _requestPublishKnowledgeUseCase = requestPublishKnowledgeUseCase;
            _deletePublicationRequestUseCase = deletePublicationRequestUseCase;
            _getPublicationRequestsUseCase = getPublicationRequestsUseCase;
            _approveRejectPublicationRequestUseCase = approveRejectPublicationRequestUseCase;
            _updateKnowledgeVisibilityUseCase = updateKnowledgeVisibilityUseCase;
        }

        public Task<Result<PublicationRequestDto>> RequestPublishKnowledge(RequestPublishKnowledgeParams parameters)
        {
            return _requestPublishKnowledgeUseCase.Execute(parameters);
        }

        public Task<Result<PublicationRequestDto>> DeletePublicationRequest(Guid guid)
        {
            return _deletePublicationRequestUseCase.Execute(guid);
        }

        public Task<Result<IEnumerable<PublicationRequestDto>>> GetPublicationRequests(GetPublicationRequestsParams parameters)
        {
            return _getPublicationRequestsUseCase.Execute(parameters);
        }

        public Task<Result<PublicationRequestDto>> ApproveRejectPublicationRequest(ApproveRejectPublicationRequestParams parameters)
        {
            return _approveRejectPublicationRequestUseCase.Execute(parameters);
        }

        public Task<Result<KnowledgeDto>> UpdateKnowledgeVisibility(UpdateKnowledgeVisibilityParams Params)
        {
            return _updateKnowledgeVisibilityUseCase.Execute(Params);
        }
    }
}