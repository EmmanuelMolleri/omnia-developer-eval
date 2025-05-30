
using Domain.Dto;
using MediatR;

namespace Application.Queries.Categories;

public class GetCategoryRatingsQuery: IRequest<BaseResult<List<ProductDto>>>
{
    public string Category { get; set; }
}
