using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Prueba.Infrastructure.Common.OptionModels;

namespace Prueba.Infrastructure.Common.Connections;

public interface IRickAndMortyApiClient
{
    /// <summary>
    /// Realiza una petición GET a un endpoint específico
    /// </summary>
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken);
}

public sealed class RickAndMortyApiClient(IHttpClientFactory clientFactory,
                                          IOptions<RickAndMortyApiOptions> options) 
    : IRickAndMortyApiClient
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(options.Value.ClientName);

    public async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("El endpoint no puede estar vacío", nameof(endpoint));

        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return JsonConvert.DeserializeObject<T>(responseContent);
    }
}
