using Domain.Dto;
using MediatR;

namespace Application.Commands.Cart;

public class DeleteCartCommand: IRequest<BaseResult<string>>
{
    public int Id { get; set; }
}
