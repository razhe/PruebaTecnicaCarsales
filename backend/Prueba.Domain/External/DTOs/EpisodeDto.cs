using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record EpisodeFilterDto
{
    public string? Name { get; init; }

    /// <summary>
    /// Formato: S01E01
    /// </summary>
    public string? Episode { get; init; }

    public int? Page { get; init; }
}

public sealed record EpisodeDto
{
    [JsonProperty("id")]
    public required int Id { get; init; }

    [JsonProperty("name")]
    public required string Name { get; init; }

    [JsonProperty("air_date")]
    public required string AirDate { get; init; }

    [JsonProperty("episode")]
    public required string EpisodeCode { get; init; }

    [JsonProperty("characters")]
    public required string[] Characters { get; init; }

    [JsonProperty("url")]
    public required string Url { get; init; }

    [JsonProperty("created")]
    public required string Created { get; init; }
}
