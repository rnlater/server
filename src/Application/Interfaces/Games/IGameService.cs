using Application.DTOs;
using Application.DTOs.SingleIdPivotEntities;
using Application.UseCases.Games;
using Shared.Types;

namespace Application.Interfaces.Games
{
    public interface IGameService
    {
        /// <summary>
        /// Create a game
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the created game</returns>
        /// <exception cref="ErrorMessage.StoreFileError">Thrown when the image file is not stored</exception>
        Task<Result<GameDto>> CreateGame(CreateGameParams parameters);

        /// <summary>
        /// Update a game
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the updated game</returns>
        /// <exception cref="ErrorMessage.NoGameFoundWithGuid">Thrown when the game is not found</exception>
        /// <exception cref="ErrorMessage.DeleteFileError">Thrown when the image file is not deleted</exception>
        /// <exception cref="ErrorMessage.StoreFileError">Thrown when the image file is not stored</exception>
        Task<Result<GameDto>> UpdateGame(UpdateGameParams parameters);

        /// <summary>
        /// Delete a game
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return result of the deleted game</returns>
        /// <exception cref="ErrorMessage.NoGameFoundWithGuid">Thrown when the game is not found</exception>
        /// <exception cref="ErrorMessage.DeleteFileError">Thrown when the image file is not deleted</exception>
        Task<Result<GameDto>> DeleteGame(Guid id);

        /// <summary>
        /// Get all games
        /// </summary>
        /// <returns>return result of the list of games</returns>
        /// <exception cref="ErrorMessage.NoGamesFound">Thrown when there are no games</exception>
        Task<Result<IEnumerable<GameDto>>> GetAllGames();

        /// <summary>
        /// Get a game by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return result of the game</returns>
        /// <exception cref="ErrorMessage.NoGameFoundWithGuid">Thrown when the game is not found</exception>
        Task<Result<GameDto>> GetGameByGuid(Guid id);

        /// <summary>
        /// Attach a game to a knowledge and create game options
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the game knowledge subscription</returns>
        /// <exception cref="ErrorMessage.RequireExactOneQuestion">Thrown when there is no  or many question game options</exception>
        /// <exception cref="ErrorMessage.RequireAtLeastTwoAnswers">Thrown when there are less than 2 answer game options</exception>
        /// <exception cref="ErrorMessage.RequireExactOneCorrectAnswer">Thrown when there is no or more than one correct answer game options</exception>
        /// <exception cref="ErrorMessage.GameKnowledgeSubscriptionAlreadyExists">Thrown when the game knowledge subscription already exists</exception>
        Task<Result<GameKnowledgeSubscriptionDto>> AttachGameToKnowledge(AttachGameToKnowledgeParams parameters);

    }
}