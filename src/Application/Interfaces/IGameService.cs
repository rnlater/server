using Application.DTOs;
using Application.UseCases.Games;
using Shared.Types;

namespace Application.Interfaces
{
    public interface IGameService
    {
        Task<Result<GameDto>> CreateGame(CreateGameParams parameters);
        Task<Result<GameDto>> UpdateGame(UpdateGameParams parameters);
        Task<Result<GameDto>> DeleteGame(Guid id);
        Task<Result<IEnumerable<GameDto>>> GetAllGames();
        Task<Result<GameDto>> GetGameByGuid(Guid id);
    }
}