using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class DeleteTrackUseCase : IUseCase<TrackDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteTrackUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<Result<TrackDto>> Execute(Guid trackId)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();

            var track = await trackRepository.Find(new BaseSpecification<Track>(t => t.Id == trackId));

            if (track == null)
            {
                return Result<TrackDto>.Fail(ErrorMessage.NoTrackFoundWithGuid);
            }

            track = await trackRepository.Delete(trackId);

            return Result<TrackDto>.Done(_mapper.Map<TrackDto>(track));
        }
        catch (Exception)
        {
            return Result<TrackDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}