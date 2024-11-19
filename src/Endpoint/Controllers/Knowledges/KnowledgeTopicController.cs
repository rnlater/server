using Application.Interfaces.Knowledges;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Application.UseCases.Knowledges.KnowledgeTopics;
using Endpoint.ApiRequests.Knowledges.KnowledgeTopics;

namespace Endpoint.Controllers.Knowledges
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeTopicController : ControllerBase
    {
        private readonly IKnowledgeTopicService _knowledgeTopicService;
        private readonly IMapper _mapper;

        public KnowledgeTopicController(IKnowledgeTopicService knowledgeTopicService, IMapper mapper)
        {
            _knowledgeTopicService = knowledgeTopicService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateKnowledgeTopicRequest, CreateKnowledgeTopicParams>();
                cfg.CreateMap<UpdateKnowledgeTopicRequest, UpdateKnowledgeTopicParams>();
                cfg.CreateMap<AttachDetachKnowledgesTopicRequest, AttachDetachKnowledgesParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpGet(HttpRoute.GetKnowledgeTopicByGuid)]
        // [Authorize]
        public async Task<IActionResult> GetKnowledgeTopicByGuid(Guid id)
        {
            var result = await _knowledgeTopicService.GetKnowledgeTopicByGuid(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetKnowledgeTopics)]
        // [Authorize]
        public async Task<IActionResult> GetKnowledgeTopics()
        {
            var result = await _knowledgeTopicService.GetKnowledgeTopics();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.CreateKnowledgeTopic)]
        // [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> CreateKnowledgeTopic([FromBody] CreateKnowledgeTopicRequest request)
        {
            var Params = _mapper.Map<CreateKnowledgeTopicParams>(request);
            var result = await _knowledgeTopicService.CreateKnowledgeTopic(Params);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateKnowledgeTopic)]
        // [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> UpdateKnowledgeTopic([FromBody] UpdateKnowledgeTopicRequest request)
        {
            var Params = _mapper.Map<UpdateKnowledgeTopicParams>(request);
            var result = await _knowledgeTopicService.UpdateKnowledgeTopic(Params);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteKnowledgeTopic)]
        // [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> DeleteKnowledgeTopic(Guid id)
        {
            var result = await _knowledgeTopicService.DeleteKnowledgeTopic(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.AttachDetachKnowledges)]
        // [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> AttachDetachKnowledges([FromBody] AttachDetachKnowledgesTopicRequest request)
        {
            var Params = _mapper.Map<AttachDetachKnowledgesParams>(request);
            var result = await _knowledgeTopicService.AttachDetachKnowledges(Params);
            return result.IsSuccess ? Ok(result.Value.ToString()) : BadRequest(result.Errors);
        }
    }
}
