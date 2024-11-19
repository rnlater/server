using Application.Interfaces;
using Application.UseCases.Games;
using AutoMapper;
using Endpoint.ApiRequests.Games;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(IGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateGameRequest, CreateGameParams>();
                cfg.CreateMap<UpdateGameRequest, UpdateGameParams>();
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
    }
}