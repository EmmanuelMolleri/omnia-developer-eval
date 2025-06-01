using Application.Handlers.Categories;
using Application.Queries.Categories;
using Domain.Entities;
using Infraestructure.Repositories;
using Infraestructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ApplicationTests.Products;

public class GetCategoryRatingsQueryHandlerTests
{
    private readonly AppDbContext _context;
    private readonly GetCategoryRatingsQueryHandler _handler;

    public GetCategoryRatingsQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        var fakeProductDomain = new FakeProductDomain(_context);
        _handler = new GetCategoryRatingsQueryHandler(fakeProductDomain);
    }

    [Fact]
    public async Task Handle_ReturnsProductsInCategory_WithCorrectAggregatedRating()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 1,
            Title = "Product A",
            Price = 100,
            Description = "Test",
            Category = "electronics",
            Image = "img.jpg",
            Rating = new List<Rating>
            {
                new Rating { Rate = 4 },
                new Rating { Rate = 2 }
            }
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var query = new GetCategoryRatingsQuery
        {
            Category = "electronics",
            _page = 1,
            _size = 10
        };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.Single(result.Data.Result);
        var dto = result.Data.Result.First();
        Assert.Equal(3, dto.Rating.Rate);
        Assert.Equal(2, dto.Rating.Count);
    }

    [Fact]
    public async Task Handle_ReturnsEmpty_WhenNoProductsFound()
    {
        // Arrange
        var query = new GetCategoryRatingsQuery
        {
            Category = "nonexistent",
            _page = 1,
            _size = 10
        };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.Empty(result.Data.Result);
        Assert.Equal(HttpStatusCode.OK, result.Data.Status);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(1, result.CurrentPage);
        Assert.Equal(0, result.TotalPages);
    }

    [Fact]
    public async Task Handle_PaginatesProperly()
    {
        // Arrange
        for (int i = 1; i <= 25; i++)
        {
            _context.Products.Add(new Product
            {
                ProductId = i,
                Title = $"Product {i}",
                Price = i * 10,
                Description = "Test",
                Category = "books",
                Image = "img.jpg",
                Rating = new List<Rating> { new Rating { Rate = 5 } }
            });
        }
        await _context.SaveChangesAsync();

        var query = new GetCategoryRatingsQuery
        {
            Category = "books",
            _page = 2,
            _size = 10
        };

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.Equal(25, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(10, result.Data.Result.Count);
        Assert.Equal("Product 11", result.Data.Result.First().Title);
    }
}

// Fake implementation for IProductDomain
public class FakeProductDomain : IProductDomain
{
    public DbSet<Product> Products { get; set; }

    public FakeProductDomain(AppDbContext context)
    {
        Products = context.Products;
    }
}
