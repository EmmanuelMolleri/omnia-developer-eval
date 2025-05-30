
using Application.Commands.Auth;
using Application.Handlers.Auth;
using MediatR;

namespace IoC;

public static class AddServicesDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddMediatR(typeof(AuthCommandHandler).Assembly);
        services.AddMediatR(typeof(AuthCommand).Assembly);

        return services;
    }
}