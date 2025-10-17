using Microsoft.Extensions.DependencyInjection;

namespace Prueba.Domain.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}
