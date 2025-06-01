using System;
using System.Net;
using Application.Queries.Products;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Products;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, BaseResult<ProductDto?>>
{
    private readonly IProductDomain _productDomain;

    public GetProductByIdQueryHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BaseResult<ProductDto?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productDomain.Products
            .Include(p => p.Rating)
            .FirstOrDefaultAsync(p => p.ProductId == request.Id, cancellationToken);

        if (product == null)
        {
            return new BaseResult<ProductDto?>
            {
                Status = HttpStatusCode.NoContent,
                Result = null
            };
        }

        return new BaseResult<ProductDto?>
        {
            Status = HttpStatusCode.OK,
            Result = new ProductDto
            {
                ProductId = product.ProductId,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Image = product.Image,
                Rating = new RatingDto
                {
                    Count = product.Rating.Count(),
                    Rate = product.Rating.Any() ? product.Rating.Sum(x => x.Rate) / product.Rating.Count() : 0
                }
            }
        };
    }
}