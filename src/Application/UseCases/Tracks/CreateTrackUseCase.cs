using Application.DTOs;
using AutoMapper;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class CreateTrackParams
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<Guid> SubjectGuids { get; set; } = [];
}

public class CreateTrackUseCase : IUseCase<TrackDto, CreateTrackParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTrackUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TrackDto>> Execute(CreateTrackParams parameters)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();

            var track = new Track
            {
                Name = parameters.Name,
                Description = parameters.Description,
            };
            track = await trackRepository.Add(track);

            List<Subject> subjects = [];
            var subjectReposotory = _unitOfWork.Repository<Subject>();
            foreach (var subjectGuid in parameters.SubjectGuids)
            {
                var subject = await subjectReposotory.GetById(subjectGuid);
                if (subject == null)
                {
                    return Result<TrackDto>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
                }

                subjects.Add(subject);
            }

            var trackSubjectRepository = _unitOfWork.Repository<TrackSubject>();
            foreach (var subject in subjects)
            {
                await trackSubjectRepository.Add(new TrackSubject
                {
                    TrackId = track.Id,
                    SubjectId = subject.Id,
                });
            }

            return Result<TrackDto>.Done(_mapper.Map<TrackDto>(track));
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<TrackDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
