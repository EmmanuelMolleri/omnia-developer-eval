namespace ApplicationTests.Carts;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Handlers.Cart;
using Application.Queries.Cart;
using Bogus;
using Domain.Entities;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using NSubstitute;
using Xunit;

public class GetQueryHandlerTests
{
    private readonly ICartDomain _cartDomain;
    private readonly GetQueryHandler _handler;

    public GetQueryHandlerTests()
    {
        _cartDomain = Substitute.For<ICartDomain>();
        _handler = new GetQueryHandler(_cartDomain);
    }

    [Fact]
    public async Task Handle_ReturnsPaginatedResult_WithDefaultOrder()
    {
        // Arrange
        var carts = GetFakeCarts(3);
        var queryable = carts.AsQueryable().BuildMock().BuildMockDbSet();

        _cartDomain.Carts.Returns(queryable);

        var request = new GetQuery
        {
            _page = 1,
            _size = 2
        };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, result.Status);
        Assert.Equal(3, result.Result.TotalItems);
        Assert.Equal(2, result.Result.Data.Count);
    }

    [Fact]
    public async Task Handle_AppliesFiltersCorrectly()
    {
        var carts = GetFakeCarts(5);
        carts[2].UserId = 1;

        var queryable = carts.AsQueryable().BuildMock().BuildMockDbSet();
        _cartDomain.Carts.Returns(queryable);

        var request = new GetQuery
        {
            _page = 1,
            _size = 10,
            Filters = new Dictionary<string, string> { { "UserId", "1" } }
        };

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Single(result.Result.Data);
        Assert.Equal(1, result.Result.Data[0].UserId);
    }

    [Fact]
    public async Task Handle_AppliesWildcardFilter()
    {
        var carts = GetFakeCarts(4);
        carts[1].UserId = 12345;

        var queryable = carts.AsQueryable().BuildMock().BuildMockDbSet();
        _cartDomain.Carts.Returns(queryable);

        var request = new GetQuery
        {
            _page = 1,
            _size = 10,
            Filters = new Dictionary<string, string> { { "UserId", "12345" } } 
        };

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Single(result.Result.Data);
        Assert.Equal(12345, result.Result.Data[0].UserId);
    }

    [Fact]
    public async Task Handle_AppliesMinMaxFilters()
    {
        var carts = GetFakeCarts(5);

        carts[0].CartId = 1;
        carts[1].CartId = 2;
        carts[2].CartId = 3;
        carts[3].CartId = 0; 
        carts[4].CartId = 0;

        var queryable = carts.AsQueryable().BuildMock().BuildMockDbSet();
        _cartDomain.Carts.Returns(queryable);

        var request = new GetQuery
        {
            _page = 1,
            _size = 10,
            Filters = new Dictionary<string, string> { { "_minCartId", "2" } }
        };

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(2, result.Result.TotalItems);
    }


    [Fact]
    public async Task Handle_AppliesOrderBy_WithEnumDescription()
    {
        var carts = GetFakeCarts(2);
        carts[0].CartId = 99;
        carts[1].CartId = 10;

        var queryable = carts.AsQueryable().BuildMock().BuildMockDbSet();
        _cartDomain.Carts.Returns(queryable);

        var request = new GetQuery
        {
            _page = 1,
            _size = 10,
            _order = "CartId desc"
        };

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(99, result.Result.Data.First().CartId);
    }

    private List<Cart> GetFakeCarts(int count)
    {
        var productFaker = new Faker<CartProduct>()
            .RuleFor(cp => cp.ProductId, f => f.Random.Int(1))
            .RuleFor(cp => cp.Quantity, f => f.Random.Int(1, 5))
            .RuleFor(cp => cp.Product, f => new Product { ProductId = f.Random.Int(1) });

        var cartFaker = new Faker<Cart>()
            .RuleFor(c => c.CartId, f => f.Random.Int(1, 100))
            .RuleFor(c => c.UserId, f => f.Random.Int(1, 100))
            .RuleFor(c => c.Data, f => f.Date.Past())
            .RuleFor(c => c.CartProducts, f => productFaker.Generate(f.Random.Int(1, 3)));

        return cartFaker.Generate(count);
    }
}

public static class MockQueryableExtensions
{
    public static DbSet<T> BuildMockDbSet<T>(this IQueryable<T> data) where T : class
    {
        var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();
        ((IQueryable<T>)mockSet).Provider.Returns(data.Provider);
        ((IQueryable<T>)mockSet).Expression.Returns(data.Expression);
        ((IQueryable<T>)mockSet).ElementType.Returns(data.ElementType);
        ((IQueryable<T>)mockSet).GetEnumerator().Returns(data.GetEnumerator());
        return mockSet;
    }
}
