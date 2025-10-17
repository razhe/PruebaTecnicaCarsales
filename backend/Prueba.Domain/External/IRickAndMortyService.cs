using Prueba.Domain.External.DTOs;

namespace Prueba.Domain.External;

public interface IRickAndMortyService
{
    // Endpoints de Character
    Task<PaginatedResponseDto<CharacterDto>?> GetCharactersAsync(
        CharacterFilterDto? filter = null,
        CancellationToken cancellationToken = default);

    Task<CharacterDto?> GetCharacterByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<CharacterDto>?> GetMultipleCharactersAsync(
        int[] ids,
        CancellationToken cancellationToken = default);

    // Endpoints de Location
    Task<PaginatedResponseDto<LocationDto>?> GetLocationsAsync(
        LocationFilterDto? filter = null,
        CancellationToken cancellationToken = default);

    Task<LocationDto?> GetLocationByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<LocationDto>?> GetMultipleLocationsAsync(
        int[] ids,
        CancellationToken cancellationToken = default);

    // Endpoints de Episode
    Task<PaginatedResponseDto<EpisodeDto>?> GetEpisodesAsync(
        EpisodeFilterDto? filter = null,
        CancellationToken cancellationToken = default);

    Task<EpisodeDto?> GetEpisodeByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EpisodeDto>?> GetMultipleEpisodesAsync(
        int[] ids,
        CancellationToken cancellationToken = default);
}
