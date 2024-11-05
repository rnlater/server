using System;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Subjects;

public class DeleteSubjectUseCase : IUseCase<SubjectDto, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;

    public DeleteSubjectUseCase(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<SubjectDto>> Execute(Guid id)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();
            var subject = await subjectRepository.Find(new BaseSpecification<Subject>(s => s.Id == id));

            if (subject == null)
            {
                return Result<SubjectDto>.Fail(ErrorMessage.NoSubjectFoundWithGuid);
            }

            subject = await subjectRepository.Delete(id);

            if (subject != null && !string.IsNullOrEmpty(subject.Photo))
            {
                var deleteFileResult = _fileStorageService.DeleteFile(subject.Photo);
                if (!deleteFileResult.IsSuccess)
                {
                    return Result<SubjectDto>.Fail(ErrorMessage.DeleteFileError);
                }
            }

            return Result<SubjectDto>.Done(_mapper.Map<SubjectDto>(subject));
        }
        catch (Exception)
        {
            return Result<SubjectDto>.Fail(ErrorMessage.UnknownError);
        }
    }
}
