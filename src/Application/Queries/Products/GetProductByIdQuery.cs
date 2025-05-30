using Domain.Dto;
using MediatR;

namespace Application.Queries.Products;

public class GetProductByIdQuery:  BasePagination, IRequest<BaseResult<ProductDto?>>
{
    public int Id { get; set; }
}
