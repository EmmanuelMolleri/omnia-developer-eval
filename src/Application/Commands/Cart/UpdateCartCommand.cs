using System.Text.Json.Serialization;
using Domain.Dto;
using MediatR;

namespace Application.Commands.Cart;

public class UpdateCartCommand: IRequest<BaseResult<CartDto>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Data { get; set; }
    public List<ProductDto> Products { get; set; } = [];
}
