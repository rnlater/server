using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.PivotEntities;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Subjects;
public class CreateSubjectParams
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required IFormFile Photo { get; set; }
    public IEnumerable<Guid> TrackUids { get; set; } = [];
    public IEnumerable<Guid> KnowledgeUids { get; set; } = [];
}

public class CreateSubjectUseCase : IUseCase<SubjectDto, CreateSubjectParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public CreateSubjectUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<SubjectDto>> Execute(CreateSubjectParams parameters)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();

            var trackRepository = _unitOfWork.Repository<Track>();
            var knowledgeRepository = _unitOfWork.Repository<Knowledge>();

            List<Track> tracks = [];
            List<Knowledge> knowledges = [];

            foreach (var trackUid in parameters.TrackUids)
            {
                var track = await trackRepository.Find(new BaseSpecification<Track>(t => t.Id == trackUid));
                if (track == null)
                {
                    return Result<SubjectDto>.Fail(ErrorMessage.NoTrackFoundWithGuid);
                }
                tracks.Add(track);
            }

            foreach (var knowledgeUid in parameters.KnowledgeUids)
            {
                var knowledge = await knowledgeRepository.Find(new BaseSpecification<Knowledge>(k => k.Id == knowledgeUid));
                if (knowledge == null)
                {
                    return Result<SubjectDto>.Fail(ErrorMessage.NoKnowledgeFoundWithGuid);
                }
                knowledges.Add(knowledge);
            }

            var photoStoredResult = await _fileStorageService.StoreFile(parameters.Photo, "subjects");

            if (!photoStoredResult.IsSuccess)
            {
                return Result<SubjectDto>.Fail(photoStoredResult.Error);
            }

            var newSubject = new Subject
            {
                Id = Guid.NewGuid(),
                Name = parameters.Name,
                Description = parameters.Description,
                Photo = photoStoredResult.Value
            };

            foreach (var track in tracks)
            {
                newSubject.TrackSubjects.Add(new TrackSubject { TrackId = track.Id, SubjectId = newSubject.Id });
            }

            foreach (var knowledge in knowledges)
            {
                newSubject.SubjectKnowledges.Add(new SubjectKnowledge { KnowledgeId = knowledge.Id, SubjectId = newSubject.Id });
            }

            await subjectRepository.Add(newSubject);

            var subjectDto = _mapper.Map<SubjectDto>(newSubject);
            return Result<SubjectDto>.Done(subjectDto);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<SubjectDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
