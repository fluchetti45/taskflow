using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Controllers
{
    /// <summary>
    /// Controlador para gestionar los roles en la aplicación.
    /// Solo accesible por administradores.
    /// </summary>
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        /// <summary>
        /// Constructor para inicializar el controlador con el servicio de roles.
        /// </summary>
        /// <param name="roleService">Servicio que maneja las operaciones relacionadas con los roles.</param>
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Obtiene la lista de todos los roles disponibles en la aplicación.
        /// Solo puede ser ejecutado por administradores.
        /// </summary>
        /// <returns>Lista de roles.</returns>
        /// <response code="200">Lista de roles obtenida con éxito.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<IEnumerable<AppRole>>> GetRoles()
        {
            IEnumerable<AppRole> roles = await _roleService.GetRoles();
            return Ok(roles);
        }

        /// <summary>
        /// Obtiene los detalles de un rol específico por su ID.
        /// Solo accesible por administradores.
        /// </summary>
        /// <param name="id">ID del rol a obtener.</param>
        /// <returns>Detalles del rol solicitado.</returns>
        /// <response code="200">Rol encontrado y retornado.</response>
        /// <response code="404">Rol no encontrado.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<AppRole>> GetRole([FromRoute] int id)
        {
            AppRole role = await this._roleService.GetRole(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        /// <summary>
        /// Crea un nuevo rol en la aplicación.
        /// Solo puede ser ejecutado por administradores.
        /// </summary>
        /// <param name="createRoleDTO">Datos necesarios para crear un nuevo rol.</param>
        /// <returns>El rol creado.</returns>
        /// <response code="201">Rol creado con éxito.</response>
        /// <response code="400">Error al crear el rol.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // Código de estado 201: Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<AppRole>> CreateRole([FromBody] CreateRoleDTO createRoleDTO)
        {
            AppRole role = await this._roleService.CreateRole(createRoleDTO);
            if (role == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        /// <summary>
        /// Elimina un rol por su ID.
        /// Solo puede ser ejecutado por administradores.
        /// </summary>
        /// <param name="id">ID del rol a eliminar.</param>
        /// <returns>Estado de la operación de eliminación.</returns>
        /// <response code="204">Rol eliminado con éxito.</response>
        /// <response code="400">Error al intentar eliminar el rol.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Código de estado 204: No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult> DeleteRole([FromRoute] int id)
        {
            var result = await this._roleService.DeleteRole(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
