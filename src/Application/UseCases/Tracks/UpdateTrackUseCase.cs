using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class UpdateTrackParams
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

public class UpdateTrackUseCase : IUseCase<TrackDto, UpdateTrackParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTrackUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<TrackDto>> Execute(UpdateTrackParams parameters)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();

            var track = await trackRepository.Find(
                new BaseSpecification<Track>(t => t.Id == parameters.Id));

            if (track == null)
            {
                return Result<TrackDto>.Fail(ErrorMessage.NoTrackFoundWithGuid);
            }

            track.Name = parameters.Name;
            track.Description = parameters.Description;

            track = await trackRepository.Update(track);

            return Result<TrackDto>.Done(_mapper.Map<TrackDto>(track));
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<TrackDto>.Fail(ErrorMessage.UnknownError);
        }

    }
}
