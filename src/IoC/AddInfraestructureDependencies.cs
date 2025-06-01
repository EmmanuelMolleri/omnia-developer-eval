using Infraestructure.Repositories;
using Infraestructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IoC;

public static class AddInfraestructureDependencies
{
    public static IServiceCollection AddInfraestructureServicesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IProductDomain, AppDbContext>();
        services.AddScoped<IAuthDomain, AppDbContext>();
        services.AddScoped<ICartDomain, AppDbContext>();

        return services;
    }
}