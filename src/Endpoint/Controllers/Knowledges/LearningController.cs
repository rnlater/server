using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges.Learnings;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Knowledges.Learnings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Utils;

namespace Endpoint.Controllers.Knowledges
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly ILearningService _learningService;
        private readonly IMapper _mapper;

        public LearningController(ILearningService learningService, IMapper mapper)
        {
            _learningService = learningService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LearnKnowledgeRequest, LearnKnowledgeParams>();
                cfg.CreateMap<GetLearningsToReviewRequest, GetLearningsToReviewParams>();
                cfg.CreateMap<ReviewLearningRequest, ReviewLearningParams>();
                cfg.CreateMap<GetCurrentUserLearningsRequest, GetCurrentUserLearningsParams>()
                    .ForMember(dest => dest.LearningLevel, opt => opt.MapFrom(src => TypeConverter.StringToEnum<LearningLevel>(src.LearningLevel)));
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.LearnKnowledge)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> LearnKnowledge([FromBody] LearnKnowledgeRequests request)
        {
            var parameters = _mapper.Map<List<LearnKnowledgeParams>>(request.GroupedKnowledges);
            var result = await _learningService.LearnKnowledge(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.GetLearningsToReview)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetLearningsToReview([FromBody] GetLearningsToReviewRequest request)
        {
            var parameters = _mapper.Map<GetLearningsToReviewParams>(request);
            var result = await _learningService.GetLearningsToReview(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.ReviewLearning)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> ReviewLearning([FromBody] ReviewLearningRequests request)
        {
            var parameters = _mapper.Map<List<ReviewLearningParams>>(request.GroupedKnowledges);
            var result = await _learningService.ReviewLearning(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.GetCurrentUserLearnings)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetCurrentUserLearnings([FromBody] GetCurrentUserLearningsRequest request)
        {
            var parameters = _mapper.Map<GetCurrentUserLearningsParams>(request);
            var result = await _learningService.GetCurrentUserLearnings(parameters);
            return result.IsSuccess ? Ok(new
            {
                data = result.Value,
                paging = result.Paging
            }) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetUnlistedLearnings)]
        [Authorize(Roles = nameof(Role.User))]
        public async Task<IActionResult> GetUnlistedLearnings()
        {
            var result = await _learningService.GetUnlistedLearnings();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}