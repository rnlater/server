using Application.DTOs;
using Application.UseCases.Knowledges.PublicationRequests;
using Shared.Types;

namespace Application.Interfaces.Knowledges;

public interface IPublicationRequestService
{
    /// <summary>
    /// Request to publish a knowledge item.
    /// </summary>
    /// <param name="Params">Parameters for requesting the publication of a knowledge item.</param>
    /// <returns>Result containing the publication request.</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with the specified GUID</exception>
    /// <exception cref="ErrorMessage.UserNotAuthorized">User not authorized to request publication</exception>
    /// <exception cref="ErrorMessage.KnowledgeAlreadyRequestedForPublication">Knowledge already requested for publication</exception>
    Task<Result<PublicationRequestDto>> RequestPublishKnowledge(RequestPublishKnowledgeParams Params);

    /// <summary>
    /// Delete a publication request by its GUID.
    /// </summary>
    /// <param name="guid">The GUID of the publication request to delete.</param>
    /// <returns>Result containing the deleted publication request.</returns>
    /// <exception cref="ErrorMessage.NoPublicationRequestFoundWithGuid">No publication request found with the specified GUID</exception>
    /// <exception cref="ErrorMessage.UserNotAuthorized">User not authorized to delete the publication request</exception>
    /// <exception cref="ErrorMessage.PublicationRequestAlreadyApproved">Publication request already approved</exception>
    Task<Result<PublicationRequestDto>> DeletePublicationRequest(Guid guid);

    /// <summary>
    /// Get a list of publication requests with optional filtering and pagination.
    /// </summary>
    /// <param name="Params">Parameters for filtering and pagination.</param>
    /// <returns>Result containing a list of publication requests.</returns>
    /// <exception cref="ErrorMessage.NoPublicationRequestsFound">No publication requests found</exception>
    Task<Result<IEnumerable<PublicationRequestDto>>> GetPublicationRequests(GetPublicationRequestsParams Params);

    /// <summary>
    /// Approve or reject a publication request.
    /// </summary>
    /// <param name="Params">Parameters for approving or rejecting the publication request.</param>
    /// <returns>Result containing the updated publication request.</returns>
    /// <exception cref="ErrorMessage.NoPublicationRequestFoundWithGuid">No publication request found with the specified GUID</exception>
    /// <exception cref="ErrorMessage.PublicationRequestAlreadyApprovedOrRejected">Publication request already approved or rejected</exception>
    Task<Result<PublicationRequestDto>> ApproveRejectPublicationRequest(ApproveRejectPublicationRequestParams Params);

    /// <summary>
    /// Update the visibility of a knowledge item.
    /// </summary>
    /// <param name="Params">Parameters for updating the visibility of a knowledge item.</param>
    /// <returns>Result containing the updated knowledge item.</returns>
    /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">No knowledge found with the specified GUID</exception>
    /// <exception cref="ErrorMessage.NoChangeDetected">No change detected in the visibility</exception>
    Task<Result<KnowledgeDto>> UpdateKnowledgeVisibility(UpdateKnowledgeVisibilityParams Params);
}
