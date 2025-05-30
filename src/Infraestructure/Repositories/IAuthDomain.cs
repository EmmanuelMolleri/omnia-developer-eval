using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories;

public interface IAuthDomain
{
    public DbSet<User> Users { get; set; }
}
