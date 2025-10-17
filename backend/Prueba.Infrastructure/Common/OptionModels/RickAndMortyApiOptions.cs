namespace Prueba.Infrastructure.Common.OptionModels;

public sealed record RickAndMortyApiOptions
{
    public static readonly string SectionName = "RickAndMortyApi";

    public string ClientName { get; init; } = "RickAndMortyApiClient";

    public string BaseUrl { get; init; } = "https://rickandmortyapi.com/api/";

    public int TimeoutSeconds { get; init; } = 30;
}
