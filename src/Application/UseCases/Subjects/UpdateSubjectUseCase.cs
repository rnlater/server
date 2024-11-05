using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Subjects;

public class UpdateSubjectParams
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public IFormFile? Photo { get; set; }
}

public class UpdateSubjectUseCase : IUseCase<SubjectDto, UpdateSubjectParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public UpdateSubjectUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<SubjectDto>> Execute(UpdateSubjectParams parameters)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();
            var subject = await subjectRepository.Find(new BaseSpecification<Subject>(s => s.Id == parameters.Id));

            if (subject == null)
            {
                return Result<SubjectDto>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
            }

            if (parameters.Photo != null)
            {
                if (!string.IsNullOrEmpty(subject.Photo))
                {
                    Result<string> deleteFileResult = _fileStorageService.DeleteFile(subject.Photo);

                    if (!deleteFileResult.IsSuccess)
                    {
                        return Result<SubjectDto>.Fail(deleteFileResult.Error);
                    }
                }

                var photoStoredResult = await _fileStorageService.StoreFile(parameters.Photo, "subjects");
                if (!photoStoredResult.IsSuccess)
                {
                    return Result<SubjectDto>.Fail(photoStoredResult.Error);
                }

                subject.Photo = photoStoredResult.Value;
            }
            else if (subject.Name == parameters.Name && subject.Description == parameters.Description)
            {
                return Result<SubjectDto>.Fail(ErrorMessage.NoChangeDetected);
            }

            subject.Name = parameters.Name;
            subject.Description = parameters.Description;

            subject = await subjectRepository.Update(subject);

            return Result<SubjectDto>.Done(_mapper.Map<SubjectDto>(subject));
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackChangesAsync();
            return Result<SubjectDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
