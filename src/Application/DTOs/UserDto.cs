using Domain.Enums;

namespace Application.DTOs;

public class UserDto : SingleIdEntityDto
{
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public string? PhotoUrl { get; set; }

    public Role Role { get; set; }

    public AuthenticationDto? Authentication { get; set; }

}
