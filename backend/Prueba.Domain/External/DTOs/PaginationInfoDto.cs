using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record PaginationInfoDto
{
    [JsonProperty("count")]
    public required int Count { get; init; }

    [JsonProperty("pages")]
    public required int Pages { get; init; }

    [JsonProperty("next")]
    public string? Next { get; init; }

    [JsonProperty("prev")]
    public string? Prev { get; init; }
}