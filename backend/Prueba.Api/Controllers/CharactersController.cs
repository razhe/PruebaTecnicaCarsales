using Microsoft.AspNetCore.Mvc;
using Prueba.Domain.External;
using Prueba.Domain.External.DTOs;

namespace Prueba.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class CharactersController(IRickAndMortyService rickAndMortyService,
                                         ILogger<CharactersController> logger) 
    : ControllerBase
{
    private readonly IRickAndMortyService _rickAndMortyService = rickAndMortyService ?? throw new ArgumentNullException(nameof(rickAndMortyService));
    private readonly ILogger<CharactersController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Obtiene todos los personajes con paginación y filtros opcionales
    /// </summary>
    /// <remarks>
    /// Parámetros de filtro:
    /// - name: Nombre del personaje
    /// - status: Alive, Dead, unknown
    /// - species: Especie del personaje
    /// - type: Tipo o subspecie
    /// - gender: Female, Male, Genderless, unknown
    /// - page: Número de página (por defecto 1)
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<CharacterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCharacters(
        [FromQuery] string? name,
        [FromQuery] string? status,
        [FromQuery] string? species,
        [FromQuery] string? type,
        [FromQuery] string? gender,
        [FromQuery] int? page,
        CancellationToken cancellationToken)
    {
        try
        {
            var filter = new CharacterFilterDto
            {
                Name = name,
                Status = status,
                Species = species,
                Type = type,
                Gender = gender,
                Page = page
            };

            var result = await _rickAndMortyService.GetCharactersAsync(filter, cancellationToken);

            if (result == null)
                return NotFound("No se encontraron personajes");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener personajes");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un personaje específico por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCharacterById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            if (id <= 0)
                return BadRequest("El ID debe ser mayor a 0");

            var result = await _rickAndMortyService.GetCharacterByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound($"Personaje con ID {id} no encontrado");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener personaje con ID {Id}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene múltiples personajes por IDs
    /// </summary>
    /// <remarks>
    /// Ejemplo: /api/v1/characters/multiple?ids=1,2,3
    /// </remarks>
    [HttpGet("multiple")]
    [ProducesResponseType(typeof(IEnumerable<CharacterDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMultipleCharacters(
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

            var result = await _rickAndMortyService.GetMultipleCharactersAsync(idArray, cancellationToken);

            if (result == null || !result.Any())
                return NotFound("No se encontraron personajes con los IDs especificados");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de validación al obtener múltiples personajes");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener múltiples personajes");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
        }
    }
}
