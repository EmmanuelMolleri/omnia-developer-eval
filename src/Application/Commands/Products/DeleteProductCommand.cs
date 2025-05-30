using Domain.Dto;
using MediatR;

namespace Application.Commands.Products;

public class DeleteProductCommand: IRequest<BaseResult<string>>
{
    public int Id { get; set; }
}