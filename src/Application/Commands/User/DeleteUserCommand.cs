using Domain.Dto;
using MediatR;

namespace Application.Commands.User;

public class DeleteUserCommand: IRequest<BaseResult<string>>
{
    public int Id { get; set; }
}