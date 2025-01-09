using Application.Interfaces.Games.GameOptions;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Games.GameOptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Utils;

namespace Endpoint.Controllers.Games
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Role.Admin))]
    public class GameOptionController : ControllerBase
    {
        private readonly IGameOptionService _gameOptionService;
        private readonly IMapper _mapper;

        public GameOptionController(IGameOptionService gameOptionService, IMapper mapper)
        {
            _gameOptionService = gameOptionService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateGameOptionRequest, CreateGameOptionParams>();
                cfg.CreateMap<GroupedGameOptionRequest, GroupedGameOption>()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TypeConverter.StringToEnum<GameOptionType>(src.Type)));
                cfg.CreateMap<CreateGroupedGameOptionRequest, CreateGroupedGameOptionParams>();
                cfg.CreateMap<UpdateGameOptionRequest, UpdateGameOptionParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.CreateGameOption)]
        public async Task<IActionResult> CreateGameOption([FromBody] CreateGameOptionRequest request)
        {
            var parameters = _mapper.Map<CreateGameOptionParams>(request);
            var result = await _gameOptionService.CreateGameOption(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.CreateGroupedGameOptions)]
        public async Task<IActionResult> CreateGroupedGameOptions([FromBody] CreateGroupedGameOptionRequest request)
        {
            var parameters = _mapper.Map<CreateGroupedGameOptionParams>(request);
            var result = await _gameOptionService.CreateGroupedGameOptions(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateGameOption)]
        public async Task<IActionResult> UpdateGameOption([FromBody] UpdateGameOptionRequest request)
        {
            var parameters = _mapper.Map<UpdateGameOptionParams>(request);
            var result = await _gameOptionService.UpdateGameOption(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteGameOption)]
        public async Task<IActionResult> DeleteGameOption(Guid id)
        {
            var result = await _gameOptionService.DeleteGameOption(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}