using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.LearningLists;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Knowledges.Learninglists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Endpoint.Controllers.Knowledges
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningListController : ControllerBase
    {
        private readonly ILearningListService _learningListService;
        private readonly IMapper _mapper;

        public LearningListController(ILearningListService learningListService, IMapper mapper)
        {
            _learningListService = learningListService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateLearningListRequest, CreateLearningListParams>();
                cfg.CreateMap<UpdateLearningListRequest, UpdateLearningListParams>();
                cfg.CreateMap<AddRemoveKnowledgesToLearningListRequest, AddRemoveKnowledgesToLearningListParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.CreateLearningList)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> CreateLearningList([FromBody] CreateLearningListRequest request)
        {
            var parameters = _mapper.Map<CreateLearningListParams>(request);
            var result = await _learningListService.CreateLearningList(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateLearningList)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> UpdateLearningList([FromBody] UpdateLearningListRequest request)
        {
            var parameters = _mapper.Map<UpdateLearningListParams>(request);
            var result = await _learningListService.UpdateLearningList(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.AddRemoveKnowledgesToLearningList)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> AddRemoveKnowledgesToLearningList([FromBody] AddRemoveKnowledgesToLearningListRequest request)
        {
            var parameters = _mapper.Map<AddRemoveKnowledgesToLearningListParams>(request);
            var result = await _learningListService.AddRemoveKnowledgesToLearningList(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetAllLearningLists)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetAllLearningLists()
        {
            var result = await _learningListService.GetAllLearningLists();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetLearningListByGuid)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetLearningListByGuid(Guid id)
        {
            var result = await _learningListService.GetLearningListByGuid(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteLearningList)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> DeleteLearningList(Guid id)
        {
            var result = await _learningListService.DeleteLearningList(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}