using Domain.Enums;

namespace Domain.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public StatusEnum Status { get; set; }
    public string Role { get; set; }
}
