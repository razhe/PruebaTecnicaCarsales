using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record LocationFilterDto
{
    public string? Name { get; init; }

    public string? Type { get; init; }

    public string? Dimension { get; init; }

    public int? Page { get; init; }
}

public sealed record LocationDto
{
    [JsonProperty("id")]
    public required int Id { get; init; }

    [JsonProperty("name")]
    public required string Name { get; init; }

    [JsonProperty("type")]
    public required string Type { get; init; }

    [JsonProperty("dimension")]
    public required string Dimension { get; init; }

    [JsonProperty("residents")]
    public required string[] Residents { get; init; }

    [JsonProperty("url")]
    public required string Url { get; init; }

    [JsonProperty("created")]
    public required string Created { get; init; }
}
