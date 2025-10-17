using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record CharacterFilterDto
{
    public string? Name { get; init; }

    /// <summary>
    /// Valores: Alive, Dead, unknown
    /// </summary>
    public string? Status { get; init; }

    public string? Species { get; init; }

    public string? Type { get; init; }

    /// <summary>
    /// Valores: Female, Male, Genderless, unknown
    /// </summary>
    public string? Gender { get; init; }

    public int? Page { get; init; }
}

public sealed record CharacterDto
{
    [JsonProperty("id")]
    public required int Id { get; init; }

    [JsonProperty("name")]
    public required string Name { get; init; }

    [JsonProperty("status")]
    public required string Status { get; init; }

    [JsonProperty("species")]
    public required string Species { get; init; }

    [JsonProperty("type")]
    public string? Type { get; init; }

    [JsonProperty("gender")]
    public required string Gender { get; init; }

    [JsonProperty("origin")]
    public required LocationReferenceDto Origin { get; init; }

    [JsonProperty("location")]
    public required LocationReferenceDto Location { get; init; }

    [JsonProperty("image")]
    public required string Image { get; init; }

    [JsonProperty("episode")]
    public required string[] Episode { get; init; }

    [JsonProperty("url")]
    public required string Url { get; init; }

    [JsonProperty("created")]
    public required string Created { get; init; }
}
