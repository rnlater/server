using Application.DTOs;
using Application.UseCases.Knowledges.LearningLists;
using Shared.Types;

namespace Application.Interfaces.Knowledges
{
    public interface ILearningListService
    {
        Task<Result<IEnumerable<LearningListDto>>> GetAllLearningLists();
        Task<Result<LearningListDto>> GetLearningListByGuid(Guid guid);
        Task<Result<LearningListDto>> CreateLearningList(CreateLearningListParams Params);
        Task<Result<LearningListDto>> UpdateLearningList(UpdateLearningListParams Params);
        Task<Result<LearningListDto>> DeleteLearningList(Guid guid);
        Task<Result<LearningListKnowledgeDto>> AddRemoveKnowledgeToLearningList(AddRemoveKnowledgeToLearningListParams Params);
    }
}