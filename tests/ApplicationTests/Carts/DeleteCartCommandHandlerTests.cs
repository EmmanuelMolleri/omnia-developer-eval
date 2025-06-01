using System.Net;
using Application.Commands.Cart;
using Application.Handlers.Cart;
using Domain.Entities;
using Infraestructure.Repositories;
using NSubstitute;

namespace ApplicationTests.Carts;

public class DeleteCartCommandHandlerTests
{
    private readonly ICartDomain _cartDomain;
    private readonly DeleteCartCommandHandler _handler;

    public DeleteCartCommandHandlerTests()
    {
        _cartDomain = Substitute.For<ICartDomain>();
        _handler = new DeleteCartCommandHandler(_cartDomain);
    }

    [Fact]
    public async Task Handle_DeletesCartSuccessfully()
    {
        var cart = new Cart { CartId = 1 };
        _cartDomain.Carts.FindAsync(Arg.Is<object[]>(x => x.Length == 1 && (int)x[0] == 1), Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult(cart));
        _cartDomain.SaveChanges().Returns(1);

        var command = new DeleteCartCommand { Id = 1 };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.Status);

        _cartDomain.Carts.Received(1).Remove(cart);
        _cartDomain.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenCartDoesNotExist()
    {
        _cartDomain.Carts.FindAsync(Arg.Any<int>()).Returns((Cart)null);

        var command = new DeleteCartCommand { Id = 999 };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, result.Status);
    }
}
