using System.Reflection;
using MediatR;

namespace IoC;

public static class AddServicesDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}