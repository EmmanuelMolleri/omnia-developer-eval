using System.Net;
using Application.Queries.Cart;
using Domain.Dto;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Cart;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BaseResult<CartDto?>>
{
    private readonly ICartDomain _cartDomain;

    public GetByIdQueryHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<CartDto?>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartDomain.Carts
            .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
            .FirstOrDefaultAsync(c => c.CartId == request.Id, cancellationToken);

        if (cart == null)
        {
            return new BaseResult<CartDto?>
            {
                Status = HttpStatusCode.NoContent,
                Result = null
            };
        }

        var dto = new CartDto
        {
            CartId = cart.CartId,
            UserId = cart.UserId,
            Data = cart.Data,
            Products = cart.CartProducts.Select(cp => new ProductResumeDto
            {
                ProductId = cp.ProductId,
                Quantity = cp.Quantity
            }).ToList()
        };

        return new BaseResult<CartDto?>
        {
            Status = HttpStatusCode.OK,
            Result = dto
        };
    }
}
