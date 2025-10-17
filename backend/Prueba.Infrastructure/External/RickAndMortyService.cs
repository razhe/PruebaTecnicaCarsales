using Prueba.Domain.External;
using Prueba.Domain.External.DTOs;
using Prueba.Infrastructure.Common.Connections;
using Prueba.Infrastructure.Common.Helpers;

namespace Prueba.Infrastructure.External;

public sealed class RickAndMortyService(IRickAndMortyApiClient apiClient) : IRickAndMortyService
{

    #region Character Methods
    public async Task<PaginatedResponseDto<CharacterDto>?> GetCharactersAsync(
        CharacterFilterDto? filter = null,
        CancellationToken cancellationToken = default)
    {
        var endpoint = RickAndMortyServiceHelpers.BuildCharacterEndpoint("character", filter);
        return await apiClient.GetAsync<PaginatedResponseDto<CharacterDto>>(endpoint, cancellationToken);
    }

    public async Task<CharacterDto?> GetCharacterByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateId(id);
        var endpoint = $"character/{id}";
        return await apiClient.GetAsync<CharacterDto>(endpoint, cancellationToken);
    }

    public async Task<IEnumerable<CharacterDto>?> GetMultipleCharactersAsync(
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateIds(ids);
        var idString = string.Join(",", ids);
        var endpoint = $"character/{idString}";
        return await apiClient.GetAsync<CharacterDto[]>(endpoint, cancellationToken);
    }
    #endregion

    #region Location Methods
    public async Task<PaginatedResponseDto<LocationDto>?> GetLocationsAsync(
        LocationFilterDto? filter = null,
        CancellationToken cancellationToken = default)
    {
        var endpoint = RickAndMortyServiceHelpers.BuildLocationEndpoint("location", filter);
        return await apiClient.GetAsync<PaginatedResponseDto<LocationDto>>(endpoint, cancellationToken);
    }

    public async Task<LocationDto?> GetLocationByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateId(id);
        var endpoint = $"location/{id}";
        return await apiClient.GetAsync<LocationDto>(endpoint, cancellationToken);
    }

    public async Task<IEnumerable<LocationDto>?> GetMultipleLocationsAsync(
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateIds(ids);
        var idString = string.Join(",", ids);
        var endpoint = $"location/{idString}";
        return await apiClient.GetAsync<LocationDto[]>(endpoint, cancellationToken);
    }
    #endregion

    #region Episode Methods
    public async Task<PaginatedResponseDto<EpisodeDto>?> GetEpisodesAsync(
        EpisodeFilterDto? filter = null,
        CancellationToken cancellationToken = default)
    {
        var endpoint = RickAndMortyServiceHelpers.BuildEpisodeEndpoint("episode", filter);
        return await apiClient.GetAsync<PaginatedResponseDto<EpisodeDto>>(endpoint, cancellationToken);
    }

    public async Task<EpisodeDto?> GetEpisodeByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateId(id);
        var endpoint = $"episode/{id}";
        return await apiClient.GetAsync<EpisodeDto>(endpoint, cancellationToken);
    }

    public async Task<IEnumerable<EpisodeDto>?> GetMultipleEpisodesAsync(
        int[] ids,
        CancellationToken cancellationToken = default)
    {
        RickAndMortyServiceHelpers.ValidateIds(ids);
        var idString = string.Join(",", ids);
        var endpoint = $"episode/{idString}";
        return await apiClient.GetAsync<EpisodeDto[]>(endpoint, cancellationToken);
    }
    #endregion
}
