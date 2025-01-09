using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Endpoint.ApiRequests.Profiles
{
    public class UpdateProfileRequest
    {
        [Required]
        public required string UserName { get; set; }

        [AllowNull]
        public IFormFile? Photo { get; set; }
    }
}