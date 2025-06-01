using Domain.Dto;
using MediatR;

namespace Application.Queries.Products;

public class GetProductsQuery: BasePagination, IRequest<BaseResult<BasePaginationResult<List<ProductDto>>>>
{
    public string? Category { get; set; }
}
