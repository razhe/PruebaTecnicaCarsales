using Microsoft.AspNetCore.Mvc;
using Prueba.Domain.External;
using Prueba.Domain.External.DTOs;

namespace Prueba.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class LocationsController(IRickAndMortyService rickAndMortyService,
                                        ILogger<LocationsController> logger) 
    : ControllerBase
{
    private readonly IRickAndMortyService _rickAndMortyService = rickAndMortyService ?? throw new ArgumentNullException(nameof(rickAndMortyService));
    private readonly ILogger<LocationsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Obtiene todas las ubicaciones con paginación y filtros opcionales
    /// </summary>
    /// <remarks>
    /// Parámetros de filtro:
    /// - name: Nombre de la ubicación
    /// - type: Tipo de ubicación
    /// - dimension: Dimensión
    /// - page: Número de página (por defecto 1)
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<LocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLocations(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] string? dimension,
        [FromQuery] int? page,
        CancellationToken cancellationToken)
    {
        try
        {
            var filter = new LocationFilterDto
            {
                Name = name,
                Type = type,
                Dimension = dimension,
                Page = page
            };

            var result = await _rickAndMortyService.GetLocationsAsync(filter, cancellationToken);

            if (result == null)
                return NotFound("No se encontraron ubicaciones");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ubicaciones");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene una ubicación específica por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLocationById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a 0");

            var result = await _rickAndMortyService.GetLocationByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound($"Ubicación con ID {id} no encontrada");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener ubicación con ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene múltiples ubicaciones por IDs
    /// </summary>
    [HttpGet("multiple")]
    [ProducesResponseType(typeof(IEnumerable<LocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMultipleLocations(
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

            var result = await _rickAndMortyService.GetMultipleLocationsAsync(idArray, cancellationToken);

            if (result == null || !result.Any())
                return NotFound("No se encontraron ubicaciones con los IDs especificados");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al obtener múltiples ubicaciones");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener múltiples ubicaciones");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }
}
