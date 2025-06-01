
using Domain.Dto;
using MediatR;

namespace Application.Queries.Categories;

public class GetCategoryRatingsQuery: BasePagination, IRequest<BasePaginationResult<BaseResult<List<ProductDto>>>>
{
    public string Category { get; set; }
}
