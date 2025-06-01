using Domain.Dto;
using MediatR;

namespace Application.Queries.Cart;

public class GetQuery: BasePagination, IRequest<BaseResult<BasePaginationResult<List<CartDto>>?>>
{

}
