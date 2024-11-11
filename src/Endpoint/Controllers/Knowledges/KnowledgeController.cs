using Application.Interfaces.Knowledges;
using Application.UseCases.Knowledges;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Knowledges;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

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
                cfg.CreateMap<SearchKnowledgesRequest, SearchKnowledgesParameters>()
                    .ForMember(dest => dest.Level, opt => opt.MapFrom(src => StringToEnum<KnowledgeLevel>(src.Level)))
                    .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => StringToEnum<SearchKnowledgesParameters.OrderByType>(src.OrderBy)));
            });
            _mapper = config.CreateMapper();
        }

        private static TEnum? StringToEnum<TEnum>(string? value) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, true, out var result))
            {
                return result;
            }
            return null;
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
            var parameters = _mapper.Map<SearchKnowledgesParameters>(request);
            var result = await _knowledgeService.SearchKnowledges(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}
