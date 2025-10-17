using Newtonsoft.Json;

namespace Prueba.Domain.External.DTOs;

public sealed record PaginatedResponseDto<T>
{
    [JsonProperty("info")]
    public required PaginationInfoDto Info { get; init; }

    [JsonProperty("results")]
    public required T[] Results { get; init; }
}
