using Domain.Dto;
using MediatR;

namespace Application.Queries.Cart;

public class GetByIdQuery: BasePagination, IRequest<BaseResult<CartDto?>>
{
    public int Id { get; set; }
}
