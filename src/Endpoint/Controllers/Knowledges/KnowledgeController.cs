using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Knowledges;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Utils;

namespace Endpoint.Controllers.Knowledges
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeController : ControllerBase
    {
        private readonly IKnowledgeService _knowledgeService;
        private readonly IMapper _mapper;

        public KnowledgeController(IKnowledgeService knowledgeService, IMapper mapper)
        {
            _knowledgeService = knowledgeService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SearchKnowledgesRequest, SearchKnowledgesParams>()
                    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => TypeConverter.StringToEnum<KnowledgeLevel>(src.Level)))
                    .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => TypeConverter.StringToEnum<SearchKnowledgesParams.OrderByType>(src.OrderBy)));
                cfg.CreateMap<GetKnowledgesRequest, GetKnowledgesParams>();
                cfg.CreateMap<CreateMaterialRequest, CreateMaterialParams>();
                cfg.CreateMap<CreateKnowledgeRequest, CreateKnowledgeParams>()
                    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => TypeConverter.StringToEnum<KnowledgeLevel>(src.Level)));
                cfg.CreateMap<UpdateKnowledgeRequest, UpdateKnowledgeParams>()
                    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => TypeConverter.StringToEnum<KnowledgeLevel>(src.Level)));
                cfg.CreateMap<AttachDeattachKnowledgeTypeRequest, AttachDeattachKnowledgeTypeParams>();
                cfg.CreateMap<AttachDeattachKnowledgeTopicRequest, AttachDeattachKnowledgeTopicParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpGet(HttpRoute.GetDetailedKnowledgeByGuid)]
        // [Authorize]
        public async Task<IActionResult> GetDetailedKnowledgeByGuid(Guid id)
        {
            var result = await _knowledgeService.GetDetailedKnowledgeByGuid(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.SearchKnowledges)]
        // [Authorize]
        public async Task<IActionResult> SearchKnowledges([FromBody] SearchKnowledgesRequest request)
        {
            var parameters = _mapper.Map<SearchKnowledgesParams>(request);
            var result = await _knowledgeService.SearchKnowledges(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.GetKnowledges)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetKnowledges([FromBody] GetKnowledgesRequest request)
        {
            var parameters = _mapper.Map<GetKnowledgesParams>(request);
            var result = await _knowledgeService.GetKnowledges(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }


        [HttpPost(HttpRoute.CreateKnowledge)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateKnowledge([FromBody] CreateKnowledgeRequest request)
        {
            var parameters = _mapper.Map<CreateKnowledgeParams>(request);
            var result = await _knowledgeService.CreateKnowledge(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateKnowledge)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateKnowledge([FromBody] UpdateKnowledgeRequest request)
        {
            var parameters = _mapper.Map<UpdateKnowledgeParams>(request);
            var result = await _knowledgeService.UpdateKnowledge(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteKnowledge)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteKnowledge(Guid id)
        {
            var result = await _knowledgeService.DeleteKnowledge(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.AttachDeattachKnowledgeType)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AttachDetachKnowledgeType([FromBody] AttachDeattachKnowledgeTypeRequest request)
        {
            var parameters = _mapper.Map<AttachDeattachKnowledgeTypeParams>(request);
            var result = await _knowledgeService.AttachDeattachKnowledgeType(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.AttachDeattachKnowledgeTopic)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AttachDetachKnowledgeTopic([FromBody] AttachDeattachKnowledgeTopicRequest request)
        {
            var parameters = _mapper.Map<AttachDeattachKnowledgeTopicParams>(request);
            var result = await _knowledgeService.AttachDeattachKnowledgeTopic(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.PublishKnowledge)]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PublishKnowledge(Guid id)
        {
            var result = await _knowledgeService.PublishKnowledge(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}
