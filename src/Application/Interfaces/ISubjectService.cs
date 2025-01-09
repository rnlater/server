using Application.DTOs;
using Application.UseCases.Subjects;
using Shared.Types;

namespace Application.Interfaces
{
    public interface ISubjectService
    {
        /// <summary>
        /// Gets a subject by its GUID.
        /// </summary>
        /// <param name="id">The GUID of the subject.</param>
        /// <returns>A result containing the subject DTO if found, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoSubjectFoundWithGuid">Thrown when no subject is found with the specified GUID.</exception>
        Task<Result<SubjectDto>> GetSubjectByGuid(Guid id);

        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <param name="Params">The parameters for filtering the subjects.</param>
        /// <returns>A result containing a list of subject DTOs if found, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoSubjectsFound">Thrown when no subject is found.</exception>
        Task<Result<IEnumerable<SubjectDto>>> GetSubjects(GetSubjectsParams Params);

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="Params">The parameters for creating the subject.</param>
        /// <returns>A result containing the created subject DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.UnknownError">Thrown when an unknown error occurs during the creation of the subject.</exception>
        /// <exception cref="ErrorMessage.NoTrackFoundWithGuid">Thrown when no track is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">Thrown when no knowledge is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.StoreFileError">Thrown when an error occurs during the storage of the file.</exception>
        Task<Result<SubjectDto>> CreateSubject(CreateSubjectParams Params);

        /// <summary>
        /// Updates an existing subject.
        /// </summary>
        /// <param name="Params">The parameters for updating the subject.</param>
        /// <returns>A result containing the updated subject DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoSubjectFoundWithGuid">Thrown when no subject is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.NoChangeDetected">Thrown when no changes are detected in the subject.</exception>
        /// <exception cref="ErrorMessage.StoreFileError">Thrown when an error occurs during the storage of the file.</exception>
        /// <exception cref="ErrorMessage.DeleteFileError">Thrown when an error occurs during the deletion of the file.</exception>
        Task<Result<SubjectDto>> UpdateSubject(UpdateSubjectParams Params);

        /// <summary>
        /// Deletes a subject by its GUID.
        /// </summary>
        /// <param name="id">The GUID of the subject to delete.</param>
        /// <returns>A result containing the deleted subject DTO if successful, otherwise an error message.</returns>
        /// <exception cref="ErrorMessage.NoSubjectFoundWithGuid">Thrown when no subject is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.DeleteFileError">Thrown when an error occurs during the deletion of the file.</exception>
        Task<Result<SubjectDto>> DeleteSubject(Guid id);

        /// <summary>
        /// Modifies the subject-knowledge pivot table. Deletes a row with the specified subject ID and knowledge ID, or creates a new one if no row exists.
        /// </summary>
        /// <param name="Params">Parameters containing the subject ID and knowledge ID.</param>
        /// <returns>Returns the result of the modification, indicating whether a row was created or deleted.</returns>
        /// <exception cref="ErrorMessage.NoSubjectFoundWithGuid">Thrown when no subject is found with the specified GUID.</exception>
        /// <exception cref="ErrorMessage.NoKnowledgeFoundWithGuid">Thrown when no knowledge is found with the specified GUID.</exception>
        Task<Result<PivotSuccessModificationType>> CreateDeleteSubjectKnowledge(CreateDeleteSubjectKnowledgeParams Params);
    }
}
