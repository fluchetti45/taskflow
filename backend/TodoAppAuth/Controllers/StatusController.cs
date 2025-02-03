using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Controllers
{
    /// <summary>
    /// Controlador para gestionar los estados de las tareas en la aplicación.
    /// Permite la obtención y creación de estados.
    /// </summary>
    [ApiController]
    [Route("api/status/")]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _statusService;

        /// <summary>
        /// Constructor para inicializar el controlador con el servicio de estados.
        /// </summary>
        /// <param name="statusService">Servicio que maneja las operaciones relacionadas con los estados de las tareas.</param>
        public StatusController(StatusService statusService)
        {
            _statusService = statusService;
        }

        /// <summary>
        /// Obtiene la lista de todos los estados de las tareas disponibles.
        /// </summary>
        /// <returns>Lista de estados.</returns>
        /// <response code="200">Lista de estados obtenida con éxito.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        public async Task<ActionResult<IEnumerable<AppStatus>>> GetStatuses()
        {
            List<AppStatus> statuses = await _statusService.GetAllStatus();
            return Ok(statuses);
        }

        /// <summary>
        /// Obtiene los detalles de un estado de tarea específico por su ID.
        /// </summary>
        /// <param name="id">ID del estado a obtener.</param>
        /// <returns>Detalles del estado solicitado.</returns>
        /// <response code="200">Estado encontrado y retornado.</response>
        /// <response code="404">Estado no encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        public async Task<ActionResult<AppStatus>> GetStatus([FromRoute] int id)
        {
            AppStatus status = await _statusService.GetStatus(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        /// <summary>
        /// Crea un nuevo estado de tarea.
        /// Solo puede ser ejecutado por administradores.
        /// </summary>
        /// <param name="statusName">Nombre del nuevo estado a crear.</param>
        /// <returns>Estado creado.</returns>
        /// <response code="201">Estado creado con éxito.</response>
        /// <response code="400">Error al crear el estado.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)] // Código de estado 201: Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult> CreateTaskStatus([FromBody] string statusName)
        {
            AppStatus status = await _statusService.CreateStatus(statusName);
            if (status == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetStatus), new { id = status.Id }, status);
        }
    }
}
