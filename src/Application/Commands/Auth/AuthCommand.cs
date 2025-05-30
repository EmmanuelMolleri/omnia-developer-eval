using Domain.Dto;
using MediatR;

namespace Application.Commands.Auth;

public class AuthCommand : IRequest<BaseResult<string>>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}
