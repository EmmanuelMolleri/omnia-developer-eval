using System;
using Domain.Dto;
using MediatR;

namespace Application.Queries.Products;

public class GetProductsQuery: IRequest<BaseResult<BasePaginationResult<ProductDto>>>
{
    public string? Category { get; set; }
    public string? _order { get; set; }
}
