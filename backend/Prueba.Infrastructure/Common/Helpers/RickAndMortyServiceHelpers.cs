using System.Web;
using Prueba.Domain.External.DTOs;

namespace Prueba.Infrastructure.Common.Helpers;

public static class RickAndMortyServiceHelpers
{
    /// <summary>
    /// Construye el endpoint de personajes con filtros
    /// </summary>
    public static string BuildCharacterEndpoint(string baseEndpoint, CharacterFilterDto? filter)
    {
        if (filter == null)
            return baseEndpoint;

        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            queryParams.Add($"name={HttpUtility.UrlEncode(filter.Name)}");

        if (!string.IsNullOrWhiteSpace(filter.Status))
            queryParams.Add($"status={HttpUtility.UrlEncode(filter.Status)}");

        if (!string.IsNullOrWhiteSpace(filter.Species))
            queryParams.Add($"species={HttpUtility.UrlEncode(filter.Species)}");

        if (!string.IsNullOrWhiteSpace(filter.Type))
            queryParams.Add($"type={HttpUtility.UrlEncode(filter.Type)}");

        if (!string.IsNullOrWhiteSpace(filter.Gender))
            queryParams.Add($"gender={HttpUtility.UrlEncode(filter.Gender)}");

        if (filter.Page.HasValue && filter.Page.Value > 0)
            queryParams.Add($"page={filter.Page.Value}");

        return queryParams.Count > 0
            ? $"{baseEndpoint}?{string.Join("&", queryParams)}"
            : baseEndpoint;
    }

    /// <summary>
    /// Construye el endpoint de ubicaciones con filtros
    /// </summary>
    public static string BuildLocationEndpoint(string baseEndpoint, LocationFilterDto? filter)
    {
        if (filter == null)
            return baseEndpoint;

        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            queryParams.Add($"name={HttpUtility.UrlEncode(filter.Name)}");

        if (!string.IsNullOrWhiteSpace(filter.Type))
            queryParams.Add($"type={HttpUtility.UrlEncode(filter.Type)}");

        if (!string.IsNullOrWhiteSpace(filter.Dimension))
            queryParams.Add($"dimension={HttpUtility.UrlEncode(filter.Dimension)}");

        if (filter.Page.HasValue && filter.Page.Value > 0)
            queryParams.Add($"page={filter.Page.Value}");

        return queryParams.Count > 0
            ? $"{baseEndpoint}?{string.Join("&", queryParams)}"
            : baseEndpoint;
    }

    /// <summary>
    /// Construye el endpoint de episodios con filtros
    /// </summary>
    public static string BuildEpisodeEndpoint(string baseEndpoint, EpisodeFilterDto? filter)
    {
        if (filter == null)
            return baseEndpoint;

        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            queryParams.Add($"name={HttpUtility.UrlEncode(filter.Name)}");

        if (!string.IsNullOrWhiteSpace(filter.Episode))
            queryParams.Add($"episode={HttpUtility.UrlEncode(filter.Episode)}");

        if (filter.Page.HasValue && filter.Page.Value > 0)
            queryParams.Add($"page={filter.Page.Value}");

        return queryParams.Count > 0
            ? $"{baseEndpoint}?{string.Join("&", queryParams)}"
            : baseEndpoint;
    }

    /// <summary>
    /// Valida que un ID sea válido
    /// </summary>
    public static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0", nameof(id));
    }

    /// <summary>
    /// Valida que el arreglo de IDs no esté vacío
    /// </summary>
    public static void ValidateIds(int[] ids)
    {
        if (ids == null || ids.Length == 0)
            throw new ArgumentException("Debe proporcionar al menos un ID", nameof(ids));

        if (ids.Any(id => id <= 0))
            throw new ArgumentException("Todos los IDs deben ser mayores a 0", nameof(ids));
    }
}
