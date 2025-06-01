using System.Net;
using Application.Handlers.Cart;
using Application.Queries.Cart;
using Domain.Entities;
using Infraestructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.Carts;

public class GetByIdHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCart_WhenCartExists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var product = new Product { ProductId = 1, /* ... */ };
        var cart = new Cart
        {
            CartId = 1,
            UserId = 10,
            Data = DateTime.UtcNow,
            CartProducts = new List<CartProduct>
            {
                new CartProduct { CartId = 1, ProductId = 1, Quantity = 2, Product = product }
            }
        };

        context.Products.Add(product);
        context.Carts.Add(cart);
        context.SaveChanges();

        var handler = new GetByIdQueryHandler(context);
        var query = new GetByIdQuery { Id = 1 };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result.Result);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.Equal(cart.CartId, result.Result.CartId);
        Assert.Single(result.Result.Products);
        Assert.Equal(2, result.Result.Products[0].Quantity);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenCartDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);
        var handler = new GetByIdQueryHandler(context);

        var query = new GetByIdQuery { Id = 999 };
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Null(result.Result);
        Assert.Equal(HttpStatusCode.NoContent, result.Status);
    }
}
