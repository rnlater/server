using Application.DTOs;
using Shared.Types;
using Application.UseCases.Knowledges.KnowledgeTopics;

namespace Application.Interfaces.Knowledges
{
    public interface IKnowledgeTopicService
    {
        Task<Result<KnowledgeTopicDto>> GetKnowledgeTopicByGuid(Guid id);
        Task<Result<IEnumerable<KnowledgeTopicDto>>> GetKnowledgeTopics();
        Task<Result<KnowledgeTopicDto>> CreateKnowledgeTopic(CreateKnowledgeTopicParams Params);
        Task<Result<KnowledgeTopicDto>> UpdateKnowledgeTopic(UpdateKnowledgeTopicParams Params);
        Task<Result<KnowledgeTopicDto>> DeleteKnowledgeTopic(Guid id);
        Task<Result<bool>> AttachDetachKnowledges(AttachDetachKnowledgesParams Params);
    }
}
