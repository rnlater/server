using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Subjects;
using Shared.Types;

namespace Application.Services;

public class SubjectService : ISubjectService
{
    private readonly CreateDeleteSubjectKnowledgeUseCase _createDeleteSubjectKnowledgeUseCase;
    private readonly CreateSubjectUseCase _createSubjectUseCase;
    private readonly DeleteSubjectUseCase _deleteSubjectUseCase;
    private readonly GetSubjectByGuidUseCase _getSubjectByGuidUseCase;
    private readonly GetSubjectsUseCase _getSubjectsUseCase;
    private readonly UpdateSubjectUseCase _updateSubjectUseCase;

    public SubjectService(
        CreateDeleteSubjectKnowledgeUseCase createDeleteSubjectKnowledgeUseCase,
        CreateSubjectUseCase createSubjectUseCase,
        DeleteSubjectUseCase deleteSubjectUseCase,
        GetSubjectByGuidUseCase getSubjectByGuidUseCase,
        GetSubjectsUseCase getSubjectsUseCase,
        UpdateSubjectUseCase updateSubjectUseCase)
    {
        _createDeleteSubjectKnowledgeUseCase = createDeleteSubjectKnowledgeUseCase;
        _createSubjectUseCase = createSubjectUseCase;
        _deleteSubjectUseCase = deleteSubjectUseCase;
        _getSubjectByGuidUseCase = getSubjectByGuidUseCase;
        _getSubjectsUseCase = getSubjectsUseCase;
        _updateSubjectUseCase = updateSubjectUseCase;
    }


    public Task<Result<PivotSuccessModificationType>> CreateDeleteSubjectKnowledge(CreateDeleteSubjectKnowledgeParams Params)
    {
        return _createDeleteSubjectKnowledgeUseCase.Execute(Params);
    }

    public Task<Result<SubjectDto>> CreateSubject(CreateSubjectParams Params)
    {
        return _createSubjectUseCase.Execute(Params);
    }

    public Task<Result<SubjectDto>> DeleteSubject(Guid id)
    {
        return _deleteSubjectUseCase.Execute(id);
    }

    public Task<Result<SubjectDto>> GetSubjectByGuid(Guid id)
    {
        return _getSubjectByGuidUseCase.Execute(id);
    }

    public Task<Result<IEnumerable<SubjectDto>>> GetSubjects(GetSubjectsParams Params)
    {
        return _getSubjectsUseCase.Execute(Params);
    }

    public Task<Result<SubjectDto>> UpdateSubject(UpdateSubjectParams Params)
    {
        return _updateSubjectUseCase.Execute(Params);
    }
}
