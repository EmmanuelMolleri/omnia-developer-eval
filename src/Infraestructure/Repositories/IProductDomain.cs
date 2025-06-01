using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public interface IProductDomain
{
    public DbSet<Product> Products { get; set; }
}
