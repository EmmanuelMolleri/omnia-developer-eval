using Domain.Entities;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.UnitOfWork;

public class AppDbContext : DbContext, IAuthDomain, ICartDomain
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Product> Products { get; set; }
    
}