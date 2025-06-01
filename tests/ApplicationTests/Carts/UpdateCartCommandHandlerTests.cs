using System.Net;
using Application.Commands.Cart;
using Application.Handlers.Cart;
using Domain.Entities;
using Infraestructure.Repositories;
using Infraestructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ApplicationTests.Carts;

public class UpdateCartCommandHandlerTests
{
    private readonly ICartDomain _cartDomain;
    private readonly UpdateCartCommandHandler _handler;

    public UpdateCartCommandHandlerTests()
    {

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _cartDomain = new AppDbContext(options);
        _handler = new UpdateCartCommandHandler(_cartDomain);
    }

    [Fact]
    public async Task Handle_UpdatesCartSuccessfully()
    {
        var cart = new Cart
        {
            CartId = 1,
            UserId = 10,
            CartProducts = new List<CartProduct>()
        };
        _cartDomain.Carts.Add(cart);
        _cartDomain.SaveChanges();

        var handler = new UpdateCartCommandHandler(_cartDomain);

        var command = new UpdateCartCommand
        {
            Id = 1,
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.Status);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenCartDoesNotExist()
    {
        var command = new UpdateCartCommand { Id = 999, UserId = 10 };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(result.Status, HttpStatusCode.NotFound);
        Assert.Equal(HttpStatusCode.NotFound, result.Status);
    }
}
