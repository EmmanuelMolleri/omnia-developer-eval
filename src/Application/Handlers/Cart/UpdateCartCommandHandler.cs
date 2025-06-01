using System.Net;
using Application.Commands.Cart;
using Domain.Dto;
using Domain.Entities;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Cart;

public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, BaseResult<CartDto>>
{
    private readonly ICartDomain _cartDomain;

    public UpdateCartCommandHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<CartDto>> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult<CartDto>();

        var cart = await _cartDomain.Carts
            .Include(c => c.CartProducts)
            .FirstOrDefaultAsync(c => c.CartId == request.Id, cancellationToken);

        if (cart == null)
        {
            result.Status = HttpStatusCode.NotFound;
            result.Messages.Add($"Cart with ID {request.Id} not found.");
            return result;
        }

        cart.UserId = request.UserId;
        cart.Data = request.Data;
        cart.CartProducts.Clear();

        if (request.Products != null && request.Products.Any())
        {
            var groupedProducts = request.Products
                .GroupBy(p => p.ProductId)
                .Select(g => new { ProductId = g.Key, Quantity = g.Count() });

            foreach (var gp in groupedProducts)
            {
                cart.CartProducts.Add(new CartProduct
                {
                    CartId = cart.CartId,
                    ProductId = gp.ProductId,
                    Quantity = gp.Quantity
                });
            }
        }

        try
        {
            _cartDomain.SaveChanges();

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

            result.Status = HttpStatusCode.OK;
            result.Result = dto;
        }
        catch (Exception ex)
        {
            result.Status = HttpStatusCode.InternalServerError;
            result.Messages.Add($"Error updating cart: {ex.Message}");
        }

        return result;
    }
}
