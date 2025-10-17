using Microsoft.AspNetCore.Mvc;
using Prueba.Domain.External;
using Prueba.Domain.External.DTOs;

namespace Prueba.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class EpisodesController(IRickAndMortyService rickAndMortyService,
                                       ILogger<EpisodesController> logger) 
    : ControllerBase
{
    private readonly IRickAndMortyService _rickAndMortyService = rickAndMortyService ?? throw new ArgumentNullException(nameof(rickAndMortyService));
    private readonly ILogger<EpisodesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Obtiene todos los episodios con paginación y filtros opcionales
    /// </summary>
    /// <remarks>
    /// Parámetros de filtro:
    /// - name: Nombre del episodio
    /// - episode: Código del episodio (ej: S01E01)
    /// - page: Número de página (por defecto 1)
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<EpisodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEpisodes(
        [FromQuery] string? name,
        [FromQuery] string? episode,
        [FromQuery] int? page,
        CancellationToken cancellationToken)
    {
        try
        {
            var filter = new EpisodeFilterDto
            {
                Name = name,
                Episode = episode,
                Page = page
            };

            var result = await _rickAndMortyService.GetEpisodesAsync(filter, cancellationToken);

            if (result == null)
                return NotFound("No se encontraron episodios");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener episodios");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un episodio específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EpisodeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEpisodeById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a 0");

            var result = await _rickAndMortyService.GetEpisodeByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound($"Episodio con ID {id} no encontrado");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener episodio con ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene múltiples episodios por IDs
    /// </summary>
    [HttpGet("multiple")]
    [ProducesResponseType(typeof(IEnumerable<EpisodeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMultipleEpisodes(
        [FromQuery(Name = "ids")] string ids,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest("Debe proporcionar al menos un ID");

            var idArray = ids.Split(',')
                .Select(id => id.Trim())
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id =>
                {
                    if (!int.TryParse(id, out var parsedId) || parsedId <= 0)
                        throw new ArgumentException($"ID inválido: {id}");
                    return parsedId;
                })
                .ToArray();

            if (idArray.Length == 0)
                return BadRequest("Debe proporcionar al menos un ID válido");

            var result = await _rickAndMortyService.GetMultipleEpisodesAsync(idArray, cancellationToken);

            if (result == null || !result.Any())
                return NotFound("No se encontraron episodios con los IDs especificados");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al obtener múltiples episodios");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener múltiples episodios");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }
}
