using Domain.Dto;
using MediatR;

namespace Application.Commands.Cart;

public class CreateCartCommand : IRequest<BaseResult<CartDto>>
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public List<ProductDto> Products { get; set; }
}
