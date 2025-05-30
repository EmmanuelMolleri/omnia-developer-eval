using System.Text.Json.Serialization;
using Domain.Dto;
using Domain.Enums;
using MediatR;

namespace Application.Commands.User;

public class UpdateUserCommand:  IRequest<BaseResult<UserDto>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public StatusEnum Status { get; set; }
    public string Role { get; set; }
}