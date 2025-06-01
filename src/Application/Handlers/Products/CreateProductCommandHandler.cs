using System.Net;
using Application.Commands.Products;
using Domain.Dto;
using Domain.Entities;
using Infraestructure.Repositories;
using MediatR;

namespace Application.Handlers.Products;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseResult<ProductDto>>
{
    private readonly IProductDomain _productDomain;

    public CreateProductCommandHandler(IProductDomain productDomain)
    {
        _productDomain = productDomain;
    }

    public async Task<BaseResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Title = request.Title,
            Price = request.Price,
            Description = request.Description,
            Category = request.Category,
            Image = request.Image,
            Rating = new List<Rating>()
        };

        await _productDomain.Products.AddAsync(entity, cancellationToken);
        _productDomain.SaveChanges();

        return new BaseResult<ProductDto>
        {
            Status = HttpStatusCode.Created,
            Result = new ProductDto
            {
                ProductId = entity.ProductId,
                Title = entity.Title,
                Price = entity.Price,
                Description = entity.Description,
                Category = entity.Category,
                Image = entity.Image,
                Rating = new RatingDto { Count = 0, Rate = 0 }
            }
        };
    }
}