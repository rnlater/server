using Application.DTOs;
using Application.UseCases.Games.GameOptions;
using Shared.Types;

namespace Application.Interfaces.Games.GameOptions
{
    public interface IGameOptionService
    {
        /// <summary>
        /// Create a game option with type of answer
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the created answer game option</returns>
        /// <exception cref="ErrorMessage.GameOptionGroupNotFound">Thrown when the group of game options is not found</exception>
        Task<Result<GameOptionDto>> CreateGameOption(CreateGameOptionParams parameters);

        /// <summary>
        /// Create a group of game options with type of question and answers
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the created game options</returns>
        /// <exception cref="ErrorMessage.RequireExactOneQuestion">Thrown when there is no  or many question game options</exception>
        /// <exception cref="ErrorMessage.RequireAtLeastTwoAnswers">Thrown when there are less than 2 answer game options</exception>
        /// <exception cref="ErrorMessage.RequireExactOneCorrectAnswer">Thrown when there is no or more than one correct answer game options</exception>
        Task<Result<List<GameOptionDto>>> CreateGroupedGameOptions(CreateGroupedGameOptionParams parameters);

        /// <summary>
        /// Update a game option
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>return result of the updated game option</returns>
        /// <exception cref="ErrorMessage.GameOptionNotFoundWithGuid">Thrown when the game option is not found</exception>
        Task<Result<GameOptionDto>> UpdateGameOption(UpdateGameOptionParams parameters);

        /// <summary>
        /// Delete a answer game option or the question game option with all its answers
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return result of the deleted game option</returns>
        /// <exception cref="ErrorMessage.GameOptionNotFoundWithGuid">Thrown when the game option is not found</exception>
        /// <exception cref="ErrorMessage.CannotDeleteCorrectAnswer">Thrown when the correct answer is tried to be deleted</exception>
        /// <exception cref="ErrorMessage.RequireAtLeastTwoAnswers">Thrown when there are less than 2 answer game options remain after deletion</exception>
        Task<Result<GameOptionDto>> DeleteGameOption(Guid id);
    }
}