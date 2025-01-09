using Application.Interfaces.Games;
using Application.UseCases.Games;
using Application.UseCases.Games.GameOptions;
using AutoMapper;
using Domain.Enums;
using Endpoint.ApiRequests.Games;
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
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;

            var config = new MapperConfiguration(static cfg =>
            {
                cfg.CreateMap<CreateGameRequest, CreateGameParams>();
                cfg.CreateMap<UpdateGameRequest, UpdateGameParams>();
                cfg.CreateMap<GroupedGameOptionRequest, GroupedGameOption>()
                   .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TypeConverter.StringToEnum<GameOptionType>(src.Type)));
                cfg.CreateMap<AttachGameToKnowledgeRequest, AttachGameToKnowledgeParams>();
            });
            _mapper = config.CreateMapper();
        }

        [HttpPost(HttpRoute.CreateGame)]
        public async Task<IActionResult> CreateGame([FromForm] CreateGameRequest request)
        {
            var parameters = _mapper.Map<CreateGameParams>(request);
            var result = await _gameService.CreateGame(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.UpdateGame)]
        public async Task<IActionResult> UpdateGame([FromForm] UpdateGameRequest request)
        {
            var parameters = _mapper.Map<UpdateGameParams>(request);
            var result = await _gameService.UpdateGame(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpDelete(HttpRoute.DeleteGame)]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var result = await _gameService.DeleteGame(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetGames)]
        public async Task<IActionResult> GetAllGames()
        {
            var result = await _gameService.GetAllGames();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet(HttpRoute.GetGameByGuid)]
        public async Task<IActionResult> GetGameById(Guid id)
        {
            var result = await _gameService.GetGameByGuid(id);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost(HttpRoute.AttachGameToKnowledge)]
        public async Task<IActionResult> AttachGameToKnowledge([FromBody] AttachGameToKnowledgeRequest request)
        {
            var parameters = _mapper.Map<AttachGameToKnowledgeParams>(request);
            var result = await _gameService.AttachGameToKnowledge(parameters);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}