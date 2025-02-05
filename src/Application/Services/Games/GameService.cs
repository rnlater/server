using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using Application.Interfaces.Games;
using Application.UseCases.Games;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services.Games
{
    public class GameService : IGameService
    {
        private readonly CreateGameUseCase _createGameUseCase;
        private readonly UpdateGameUseCase _updateGameUseCase;
        private readonly DeleteGameUseCase _deleteGameUseCase;
        private readonly GetAllGamesUseCase _getAllGamesUseCase;
        private readonly GetGameByGuidUseCase _getGameByGuidUseCase;
        private readonly AttachGameToKnowledgeUseCase _attachGameToKnowledgeUseCase;

        public GameService(
            CreateGameUseCase createGameUseCase,
            UpdateGameUseCase updateGameUseCase,
            DeleteGameUseCase deleteGameUseCase,
            GetAllGamesUseCase getAllGamesUseCase,
            GetGameByGuidUseCase getGameByGuidUseCase,
            AttachGameToKnowledgeUseCase attachGameToKnowledgeUseCase)
        {
            _createGameUseCase = createGameUseCase;
            _updateGameUseCase = updateGameUseCase;
            _deleteGameUseCase = deleteGameUseCase;
            _getAllGamesUseCase = getAllGamesUseCase;
            _getGameByGuidUseCase = getGameByGuidUseCase;
            _attachGameToKnowledgeUseCase = attachGameToKnowledgeUseCase;
        }

        public Task<Result<GameDto>> CreateGame(CreateGameParams parameters)
        {
            return _createGameUseCase.Execute(parameters);
        }

        public Task<Result<GameDto>> UpdateGame(UpdateGameParams parameters)
        {
            return _updateGameUseCase.Execute(parameters);
        }

        public Task<Result<GameDto>> DeleteGame(Guid id)
        {
            return _deleteGameUseCase.Execute(id);
        }

        public Task<Result<IEnumerable<GameDto>>> GetAllGames()
        {
            return _getAllGamesUseCase.Execute(NoParam.Value);
        }

        public Task<Result<GameDto>> GetGameByGuid(Guid id)
        {
            return _getGameByGuidUseCase.Execute(id);
        }

        public Task<Result<GameKnowledgeSubscriptionDto>> AttachGameToKnowledge(AttachGameToKnowledgeParams parameters)
        {
            return _attachGameToKnowledgeUseCase.Execute(parameters);
        }
    }
}