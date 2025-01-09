using Application.DTOs;
using Application.UseCases.Tracks;
using Shared.Types;

namespace Application.Interfaces;

public interface ITrackService
{
    /// <summary>
    /// Get tracks with optional filters
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of list tracks</returns>
    /// <exception cref="ErrorMessage.NoTrackFoundWithSearch">Thrown when no track is found with the search parameter.</exception>
    Task<Result<IEnumerable<TrackDto>>> GetTracks(GetTracksParams Params);

    /// <summary>
    /// Get tracks with detailed information including subjects and knowledges
    /// </summary>
    /// <returns>return result of list tracks with subjects and knowledges</returns>
    /// <exception cref="ErrorMessage.NoTrackFound">Thrown when no track is found.</exception>
    Task<Result<IEnumerable<TrackDto>>> GetDetailedTracks();

    /// <summary>
    /// Get a track by its guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>return result of the track found by its guid</returns>
    /// <exception cref="ErrorMessage.NoTrackFoundWithGuid">Thrown when no track is found with the guid.</exception>
    Task<Result<TrackDto>> GetTrackById(Guid guid);

    /// <summary>
    /// Create a new track
    /// </summary>
    /// <param name="Params"></param>
    /// <returns>return result of the created track</returns>
    /// <exception cref="ErrorMessage.NoSubjectFoundWithGuid">Thrown when no subject is found with the guid.</exception>
    Task<Result<TrackDto>> CreateTrack(CreateTrackParams Params);

    /// <summary>
    /// Update a track
    /// </summary>
    /// <param name="track"></param>
    /// <returns>return result of the updated track</returns>
    /// <exception cref="ErrorMessage.NoTrackFoundWithGuid">Thrown when no track is found with the guid.</exception>
    Task<Result<TrackDto>> UpdateTrack(UpdateTrackParams track);

    /// <summary>
    /// Delete a track by its guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>return result of the deleted track</returns>
    /// <exception cref="ErrorMessage.NoTrackFoundWithGuid">Thrown when no track is found with the guid.</exception>
    Task<Result<TrackDto>> DeleteTrack(Guid guid);

    /// <summary>
    /// Modify the track-subject pivot table. Deletes a row with the specified track ID and subject ID, or creates a new one if no row exists.
    /// </summary>
    /// <param name="Params">Parameters containing the track ID and subject ID.</param>
    /// <returns>Returns the result of the modification, indicating whether a row was created or deleted.</returns>
    Task<Result<PivotSuccessModificationType>> CreateDeleteTrackSubject(CreateDeleteTrackSubjectParams Params);
}
