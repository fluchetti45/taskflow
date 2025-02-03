using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Controllers
{
    /// <summary>
    /// Controlador para gestionar las tareas en la aplicación.
    /// Permite la creación, obtención, edición y eliminación de tareas.
    /// </summary>
    [ApiController]
    [Route("api/task")]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _taskService;

        /// <summary>
        /// Constructor para inicializar el controlador con el servicio de tareas.
        /// </summary>
        /// <param name="taskService">Servicio que maneja las operaciones relacionadas con las tareas.</param>
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Obtiene todas las tareas del usuario autenticado.
        /// </summary>
        /// <param name="status">Estado de la tarea (opcional).</param>
        /// <returns>Lista de tareas del usuario.</returns>
        /// <response code="200">Lista de tareas obtenida con éxito.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<IEnumerable<ReadTaskDTO>>> GetTasks([FromRoute] string status = "")
        {
            // Extraigo el userId del claim del token.
            var userId = int.Parse(User.FindFirstValue("id"));
            var tareas = await _taskService.GetTasks(userId, status);
            return Ok(tareas);
        }

        /// <summary>
        /// Obtiene una tarea específica por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea.</param>
        /// <returns>Detalles de la tarea solicitada.</returns>
        /// <response code="200">Tarea encontrada y retornada.</response>
        /// <response code="404">Tarea no encontrada.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="403">Acceso denegado. Solo admins pueden acceder.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOwner")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Código de estado 403: Forbidden
        public async Task<ActionResult<Tarea>> GetTaskById(int id)
        {
            Tarea tarea = await _taskService.GetTask(id);
            if (tarea == null)
            {
                return NotFound();
            }
            return Ok(tarea);
        }

        /// <summary>
        /// Crea una nueva tarea para el usuario autenticado.
        /// </summary>
        /// <param name="task">Datos de la tarea a crear.</param>
        /// <returns>La tarea creada.</returns>
        /// <response code="201">Tarea creada exitosamente.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)] // Código de estado 201: Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<ReadTaskDTO>> CreateTask([FromBody] CreateTaskDTO task)
        {
            // Extraigo el userId del claim del token.
            var userId = User.FindFirstValue("id");
            ReadTaskDTO tarea = await _taskService.CreateTask(task, userId);
            if (tarea != null)
            {
                return CreatedAtAction(nameof(GetTaskById), new { id = tarea.Id }, tarea);
            }
            return BadRequest("Algo salió mal..");
        }

        /// <summary>
        /// Modifica una tarea existente.
        /// </summary>
        /// <param name="id">ID de la tarea a modificar.</param>
        /// <param name="task">Datos de la tarea a modificar.</param>
        /// <returns>La tarea actualizada.</returns>
        /// <response code="200">Tarea actualizada con éxito.</response>
        /// <response code="404">Tarea no encontrada.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="403">Acceso denegado. Solo admins pueden modificar tareas.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOwner")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Código de estado 403: Forbidden
        public async Task<ActionResult> EditTask([FromRoute] int id, [FromBody] EditTaskDTO task)
        {
            Tarea tarea = await _taskService.UpdateTask(task);
            if (tarea == null)
            {
                return NotFound();
            }
            return Ok(tarea);
        }

        /// <summary>
        /// Elimina una tarea específica.
        /// </summary>
        /// <param name="id">ID de la tarea a eliminar.</param>
        /// <returns>Estado de la operación de eliminación.</returns>
        /// <response code="204">Tarea eliminada con éxito.</response>
        /// <response code="404">Tarea no encontrada.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="403">Acceso denegado. Solo admins pueden eliminar tareas.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOwner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Código de estado 204: No Content
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Código de estado 403: Forbidden
        public async Task<ActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue("id");
            bool result = await _taskService.DeleteTask(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
