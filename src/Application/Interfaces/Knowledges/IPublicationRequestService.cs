using Application.DTOs;
using Application.UseCases.Knowledges.PublicationRequests;
using Shared.Types;

namespace Application.Interfaces.Knowledges;

public interface IPublicationRequestService
{
    Task<Result<PublicationRequestDto>> RequestPublishKnowledge(RequestPublishKnowledgeParams Params);
    Task<Result<PublicationRequestDto>> DeletePublicationRequest(Guid guid);
    Task<Result<IEnumerable<PublicationRequestDto>>> GetPublicationRequests(GetPublicationRequestsParams Params);
    Task<Result<PublicationRequestDto>> ApproveRejectPublicationRequest(ApproveRejectPublicationRequestParams Params);
    Task<Result<KnowledgeDto>> UpdateKnowledgeVisibility(UpdateKnowledgeVisibilityParams Params);
}
