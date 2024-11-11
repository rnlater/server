using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.DTOs;

public class UserDto : SingleIdEntityDto
{
    public required string UserName { get; set; }

    public required string Email { get; set; }

    public string? PhotoUrl { get; set; }

    [EnumDataType(typeof(Role))]
    public string Role { get; set; } = Domain.Enums.Role.User.ToString();

    public AuthenticationDto? Authentication { get; set; }

}
