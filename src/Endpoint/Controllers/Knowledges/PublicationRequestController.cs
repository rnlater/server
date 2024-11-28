using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.PublicationRequests;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Knowledges.PublicationRequests;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Utils;

namespace Endpoint.Controllers.Knowledges
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationRequestController : ControllerBase
    {
        private readonly IPublicationRequestService _publicationRequestService;
        private readonly IMapper _mapper;

        public PublicationRequestController(IPublicationRequestService publicationRequestService, IMapper mapper)
        {
            _publicationRequestService = publicationRequestService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RequestPublishKnowledgeRequest, RequestPublishKnowledgeParams>();
                cfg.CreateMap<GetPublicationRequestsRequest, GetPublicationRequestsParams>()
                     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TypeConverter.StringToEnum<PublicationRequestStatus>(src.Status)));
                cfg.CreateMap<ApproveRejectPublicationRequestRequest, ApproveRejectPublicationRequestParams>();
                cfg.CreateMap<UpdateKnowledgeVisibilityRequest, UpdateKnowledgeVisibilityParams>()
                    .ForMember(dest => dest.Visibility, opt => opt.MapFrom(src => TypeConverter.StringToEnum<KnowledgeVisibility>(src.Visibility)));
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.RequestPublishKnowledge)]
        // [Authorize(Roles = "User")]
        public async Task<IActionResult> RequestPublishKnowledge([FromBody] RequestPublishKnowledgeRequest request)
        {
            var parameters = _mapper.Map<RequestPublishKnowledgeParams>(request);
            var result = await _publicationRequestService.RequestPublishKnowledge(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeletePublicationRequest)]
        // [Authorize(Roles = "User")]
        public async Task<IActionResult> DeletePublicationRequest(Guid id)
        {
            var result = await _publicationRequestService.DeletePublicationRequest(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetPublicationRequests)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPublicationRequests([FromQuery] GetPublicationRequestsRequest request)
        {
            var parameters = _mapper.Map<GetPublicationRequestsParams>(request);
            var result = await _publicationRequestService.GetPublicationRequests(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.ApproveRejectPublicationRequest)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRejectPublicationRequest([FromBody] ApproveRejectPublicationRequestRequest request)
        {
            var parameters = _mapper.Map<ApproveRejectPublicationRequestParams>(request);
            var result = await _publicationRequestService.ApproveRejectPublicationRequest(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateKnowledgeVisibility)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateKnowledgeVisibility([FromBody] UpdateKnowledgeVisibilityRequest request)
        {
            var parameters = _mapper.Map<UpdateKnowledgeVisibilityParams>(request);
            var result = await _publicationRequestService.UpdateKnowledgeVisibility(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}