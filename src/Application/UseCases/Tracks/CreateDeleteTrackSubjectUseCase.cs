using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Tracks;

public class CreateDeleteTrackSubjectParams
{
    public Guid TrackId { get; set; }
    public Guid SubjectId { get; set; }
}

public class CreateDeleteTrackSubjectUseCase : IUseCase<PivotSuccessModificationType, CreateDeleteTrackSubjectParams>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateDeleteTrackSubjectUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PivotSuccessModificationType>> Execute(CreateDeleteTrackSubjectParams parameters)
    {
        try
        {
            var trackRepository = _unitOfWork.Repository<Track>();
            var subjectRepository = _unitOfWork.Repository<Subject>();
            var trackSubjectRepository = _unitOfWork.Repository<TrackSubject>();

            var track = await trackRepository.Find(new BaseSpecification<Track>(t => t.Id == parameters.TrackId));
            if (track == null)
            {
                return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoTrackFoundWithGuid);
            }

            var subject = await subjectRepository.Find(new BaseSpecification<Subject>(s => s.Id == parameters.SubjectId));
            if (subject == null)
            {
                return Result<PivotSuccessModificationType>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
            }

            var trackSubject = await trackSubjectRepository.Find(
                new BaseSpecification<TrackSubject>(ts =>
                ts.TrackId == parameters.TrackId
                    && ts.SubjectId == parameters.SubjectId));

            if (trackSubject == null)
            {
                await trackSubjectRepository.Add(new TrackSubject
                {
                    TrackId = parameters.TrackId,
                    SubjectId = parameters.SubjectId
                });
                return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Created);
            }

            await trackSubjectRepository.Delete(trackSubject);
            return Result<PivotSuccessModificationType>.Done(PivotSuccessModificationType.Deleted);
        }
        catch (Exception)
        {
            return Result<PivotSuccessModificationType>.Fail(ErrorMessage.UnknownError);
        }
    }
}
