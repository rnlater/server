using System.ComponentModel.DataAnnotations;

namespace Endpoint.ApiRequests.Knowledges.Learninglists
{
    public class UpdateLearningListRequest
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string Title { get; set; }
    }
}