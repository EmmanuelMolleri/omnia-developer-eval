using System.Net;
using Application.Queries.Products;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Handlers.Products;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, BaseResult<BasePaginationResult<List<ProductDto>>>>
{
    private readonly IProductDomain _productDomain;

    public GetProductsQueryHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BaseResult<BasePaginationResult<List<ProductDto>>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productDomain.Products.Include(p => p.Rating).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request._order))
        {
            query = query.OrderBy(request._order);
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalItems / (double)request._size);

        var products = await query
            .Skip((request._page - 1) * request._size)
            .Take(request._size)
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

        return new BaseResult<BasePaginationResult<List<ProductDto>>>
        {
            Status = HttpStatusCode.OK,
            Result = new BasePaginationResult<List<ProductDto>>
            {
                Data = products,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = request._page
            }
        };
    }
}
