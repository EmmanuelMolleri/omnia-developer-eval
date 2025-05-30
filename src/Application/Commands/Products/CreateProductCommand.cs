using System;
using Domain.Dto;
using MediatR;

namespace Application.Commands.Products;

public class CreateProductCommand : IRequest<BaseResult<ProductDto>>
{
    public int ProductId { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Image { get; set; }
    public RatingDto? Rating { get; set; }

}
