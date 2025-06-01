using System.Net;
using Application.Queries.Categories;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Handlers.Categories;

public class GetCategoryRatingsQueryHandler : IRequestHandler<GetCategoryRatingsQuery, BasePaginationResult<BaseResult<List<ProductDto>>>>
{
    private readonly IProductDomain _productDomain;

    public GetCategoryRatingsQueryHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BasePaginationResult<BaseResult<List<ProductDto>>>> Handle(GetCategoryRatingsQuery request, CancellationToken cancellationToken)
    {
        var query = _productDomain.Products
            .Include(x => x.Rating)
            .Where(p => p.Category == request.Category);

        if (!string.IsNullOrWhiteSpace(request._order))
        {
            query = query.OrderBy(request._order);
        }

        var totalItems = await query.CountAsync(cancellationToken);

        query = query
            .Skip((request._page - 1) * request._size)
            .Take(request._size);

        var data = await query
            .Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                Title = p.Title,
                Price = p.Price,
                Description = p.Description,
                Category = p.Category,
                Image = p.Image,
                Rating = new RatingDto
                {
                    Count = p.Rating.Count(),
                    Rate = p.Rating.Count() == 0 ? 0 : p.Rating.Sum(x => x.Rate) / p.Rating.Count()
                }
            })
            .ToListAsync(cancellationToken);

        return new BasePaginationResult<BaseResult<List<ProductDto>>>
        {
            TotalItems = totalItems,
            CurrentPage = request._page,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request._size),
            Data = new BaseResult<List<ProductDto>>
            {
                Status = HttpStatusCode.OK,
                Result = data,
            }
        };
    }
}
