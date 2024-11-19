using Application.DTOs;
using Application.Interfaces.Games.GameOptions;
using Application.UseCases.Games.GameOptions;
using Shared.Types;

namespace Application.Services.Games
{
    public class GameOptionService : IGameOptionService
    {
        private readonly CreateGameOptionUseCase _createGameOptionUseCase;
        private readonly CreateGroupedGameOptionsUseCase _createGroupedGameOptionsUseCase;
        private readonly UpdateGameOptionUseCase _updateGameOptionUseCase;
        private readonly DeleteGameOptionUseCase _deleteGameOptionUseCase;

        public GameOptionService(
            CreateGameOptionUseCase createGameOptionUseCase,
            CreateGroupedGameOptionsUseCase createGroupedGameOptionsUseCase,
            UpdateGameOptionUseCase updateGameOptionUseCase,
            DeleteGameOptionUseCase deleteGameOptionUseCase)
        {
            _createGameOptionUseCase = createGameOptionUseCase;
            _createGroupedGameOptionsUseCase = createGroupedGameOptionsUseCase;
            _updateGameOptionUseCase = updateGameOptionUseCase;
            _deleteGameOptionUseCase = deleteGameOptionUseCase;
        }

        public Task<Result<GameOptionDto>> CreateGameOption(CreateGameOptionParams parameters)
        {
            return _createGameOptionUseCase.Execute(parameters);
        }

        public Task<Result<List<GameOptionDto>>> CreateGroupedGameOptions(CreateGroupedGameOptionParams parameters)
        {
            return _createGroupedGameOptionsUseCase.Execute(parameters);
        }

        public Task<Result<GameOptionDto>> UpdateGameOption(UpdateGameOptionParams parameters)
        {
            return _updateGameOptionUseCase.Execute(parameters);
        }

        public Task<Result<GameOptionDto>> DeleteGameOption(Guid id)
        {
            return _deleteGameOptionUseCase.Execute(id);
        }
    }
}