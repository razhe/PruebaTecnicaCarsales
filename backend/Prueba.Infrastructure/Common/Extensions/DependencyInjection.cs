using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Domain.External;
using Prueba.Infrastructure.Common.Connections;
using Prueba.Infrastructure.Common.OptionModels;
using Prueba.Infrastructure.External;

namespace Prueba.Infrastructure.Common.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Cargar opciones de configuración
        var rickAndMortyOptions = new RickAndMortyApiOptions();
        configuration.GetSection(RickAndMortyApiOptions.SectionName).Bind(rickAndMortyOptions);

        // Registrar opciones en el contenedor de dependencias
        services.Configure<RickAndMortyApiOptions>(configuration.GetSection(RickAndMortyApiOptions.SectionName));

        services.AddHttpClient(rickAndMortyOptions.ClientName, client =>
        {
            client.BaseAddress = new Uri(rickAndMortyOptions.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(rickAndMortyOptions.TimeoutSeconds);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IRickAndMortyApiClient, RickAndMortyApiClient>();
        services.AddScoped<IRickAndMortyService, RickAndMortyService>();

        return services;
    }
}
