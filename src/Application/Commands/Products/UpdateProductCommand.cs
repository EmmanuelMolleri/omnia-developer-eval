using System.Text.Json.Serialization;
using Domain.Dto;
using MediatR;

namespace Application.Commands.Products;

public class UpdateProductCommand:  IRequest<BaseResult<ProductDto>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Image { get; set; }
    public RatingDto? Rating { get; set; }
}

