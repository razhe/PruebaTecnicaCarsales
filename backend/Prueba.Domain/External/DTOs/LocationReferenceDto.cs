using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record LocationReferenceDto
{
    [JsonProperty("name")]
    public required string Name { get; init; }

    [JsonProperty("url")]
    public required string Url { get; init; }
}
