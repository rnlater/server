using Application.DTOs;
using AutoMapper;
using Domain.Base;
using Domain.Entities.SingleIdEntities;
using Domain.Interfaces;
using Shared.Constants;
using Shared.Types;

namespace Application.UseCases.Subjects;

public class GetSubjectsParams
{
    public string? Search { get; set; }
}

public class GetSubjectsUseCase : IUseCase<IEnumerable<SubjectDto>, GetSubjectsParams>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSubjectsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<SubjectDto>>> Execute(GetSubjectsParams parameters)
    {
        try
        {
            var subjectRepository = _unitOfWork.Repository<Subject>();
            var subjects = await subjectRepository.FindMany(
                new BaseSpecification<Subject>(s => string.IsNullOrEmpty(parameters.Search) || s.Name.Contains(parameters.Search)));

            if (!subjects.Any())
            {
                return Result<IEnumerable<SubjectDto>>.Fail(ErrorMessage.NoSubjectsFound);
            }

            return Result<IEnumerable<SubjectDto>>.Done(subjects.Select(_mapper.Map<SubjectDto>));
        }
        catch (Exception)
        {
            return Result<IEnumerable<SubjectDto>>.Fail(ErrorMessage.UnknownError);
        }
    }
}
