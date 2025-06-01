using System.Net;
using Application.Commands.Cart;
using Domain.Entities;
using Infraestructure.Repositories;
using NSubstitute;

namespace ApplicationTests.Carts;

public class CreateCartCommandHandlerTests
{
    private readonly ICartDomain _cartDomain;
    private readonly CreateCartCommandHandler _handler;

    public CreateCartCommandHandlerTests()
    {
        _cartDomain = Substitute.For<ICartDomain>();
        _handler = new CreateCartCommandHandler(_cartDomain);
    }

    [Fact]
    public async Task Handle_CreatesCartSuccessfully()
    {
        var command = new CreateCartCommand
        {
            UserId = 5,
        };

        _cartDomain.SaveChanges().Returns(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Created, result.Status);

        await _cartDomain.Received(1).Carts.AddAsync(Arg.Any<Cart>(), Arg.Any<CancellationToken>());
        _cartDomain.Received(1).SaveChanges();
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenSaveFails()
    {
        var command = new CreateCartCommand { UserId = 5 };
        _cartDomain.SaveChanges().Returns(0);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(HttpStatusCode.Created, result.Status);
    }
}
