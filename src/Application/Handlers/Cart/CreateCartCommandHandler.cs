using Application.Commands.Cart;
using Domain.Dto;
using Domain.Entities;
using Infraestructure.Repositories;
using MediatR;
using System.Net;
public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, BaseResult<CartDto>>
{
    private readonly ICartDomain _cartDomain;

    public CreateCartCommandHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var result = new BaseResult<CartDto>();

        var cart = new Cart
        {
            UserId = request.UserId,
            Data = request.Date,
            CartProducts = new List<CartProduct>()
        };

        
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

        _cartDomain.Carts.Add(cart);
        var changedLines =_cartDomain.SaveChanges();

        if (changedLines == 0)
        {
            result.Status = HttpStatusCode.BadRequest;
            result.Messages.Add("Internal server error");
            return result;
        }

        result.Status = HttpStatusCode.Created;
        result.Result = new CartDto
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

        return result;
    }
}
