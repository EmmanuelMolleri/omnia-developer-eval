using System;
using System.Net;
using Application.Commands.Products;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Products;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, BaseResult<ProductDto?>>
{
    private readonly IProductDomain _productDomain;

    public UpdateProductCommandHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BaseResult<ProductDto?>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDomain.Products.Include(p => p.Rating).FirstOrDefaultAsync(p => p.ProductId == request.Id, cancellationToken);
        if (product == null)
        {
            return new BaseResult<ProductDto?> { Status = HttpStatusCode.NotFound };
        }

        product.Title = request.Title;
        product.Price = request.Price;
        product.Description = request.Description;
        product.Category = request.Category;
        product.Image = request.Image;

        _productDomain.SaveChanges();

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