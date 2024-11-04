using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Tracks;
using Domain.Interfaces;
using Shared.Types;

namespace Application.Services;

public class TrackService : ITrackService
{
    private readonly GetTracksUseCase getTracksUseCase;
    private readonly GetDetailedTracksUseCase getDetailedTracksUseCase;
    private readonly GetTrackByGuidUseCase getTrackByGuidUseCase;
    private readonly CreateTrackUseCase createTrackUseCase;
    private readonly UpdateTrackUseCase updateTrackUseCase;
    private readonly DeleteTrackUseCase deleteTrackUseCase;
    private readonly CreateDeleteTrackSubjectUseCase createDeleteTrackSubjectUseCase;

    public TrackService(
        GetTracksUseCase getTracksUseCase,
        GetDetailedTracksUseCase getDetailedTracksUseCase,
        GetTrackByGuidUseCase getTrackByGuidUseCase,
        CreateTrackUseCase createTrackUseCase,
        UpdateTrackUseCase updateTrackUseCase,
        DeleteTrackUseCase deleteTrackUseCase,
        CreateDeleteTrackSubjectUseCase createDeleteTrackSubjectUseCase
    )
    {
        this.getTracksUseCase = getTracksUseCase;
        this.getDetailedTracksUseCase = getDetailedTracksUseCase;
        this.getTrackByGuidUseCase = getTrackByGuidUseCase;
        this.createTrackUseCase = createTrackUseCase;
        this.updateTrackUseCase = updateTrackUseCase;
        this.deleteTrackUseCase = deleteTrackUseCase;
        this.createDeleteTrackSubjectUseCase = createDeleteTrackSubjectUseCase;
    }

    public Task<Result<TrackDto>> CreateTrack(CreateTrackParams Params)
    {
        return createTrackUseCase.Execute(Params);
    }

    public Task<Result<TrackDto>> DeleteTrack(Guid id)
    {
        return deleteTrackUseCase.Execute(id);
    }

    public Task<Result<IEnumerable<TrackDto>>> GetDetailedTracks()
    {
        return getDetailedTracksUseCase.Execute(NoParam.Value);
    }

    public Task<Result<TrackDto>> GetTrackById(Guid id)
    {
        return getTrackByGuidUseCase.Execute(id);
    }

    public Task<Result<IEnumerable<TrackDto>>> GetTracks(GetTracksParams Params)
    {
        return getTracksUseCase.Execute(Params);
    }

    public Task<Result<PivotSuccessModificationType>> CreateDeleteTrackSubject(CreateDeleteTrackSubjectParams Params)
    {
        return createDeleteTrackSubjectUseCase.Execute(Params);
    }

    public Task<Result<TrackDto>> UpdateTrack(UpdateTrackParams track)
    {
        return updateTrackUseCase.Execute(track);
    }
}
