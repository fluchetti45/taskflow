using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Controllers
{
    /// <summary>
    /// Controlador para gestionar los usuarios en la aplicación.
    /// Permite la obtención, activación, desactivación, y eliminación de usuarios, así como la modificación de sus roles.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        /// <summary>
        /// Constructor para inicializar el controlador con el servicio de usuarios.
        /// </summary>
        /// <param name="service">Servicio que maneja las operaciones relacionadas con los usuarios.</param>
        public UserController(UserService service)
        {
            _userService = service;
        }

        /// <summary>
        /// Obtiene todos los usuarios activos.
        /// Solo puede ser accedido por administradores.
        /// </summary>
        /// <returns>Lista de usuarios con datos básicos.</returns>
        /// <response code="200">Lista de usuarios obtenida con éxito.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<IEnumerable<UserDataDTO>>> GetUsers()
        {
            IEnumerable<AppUser> users = await this._userService.GetUsers();
            // Mapeo los datos
            IEnumerable<UserDataDTO> usersData = users.Select(user => new UserDataDTO
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName,
            });
            return Ok(usersData);
        }

        /// <summary>
        /// Obtiene los detalles de un usuario específico por su ID.
        /// Tanto administradores como usuarios pueden acceder a esta operación si son el propio usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Datos del usuario solicitado.</returns>
        /// <response code="200">Usuario encontrado y retornado.</response>
        /// <response code="404">Usuario no encontrado.</response>
        /// <response code="401">No autorizado.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<UserDataDTO>> GetUser([FromRoute] int id)
        {
            AppUser user = await this._userService.GetUser(id);
            if (user == null || user.IsActive == false)
            {
                return NotFound();
            }
            // Mapeo los datos
            UserDataDTO userData = new UserDataDTO
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName,
            };
            return Ok(userData);
        }

        /// <summary>
        /// Reactiva un usuario que ha sido desactivado.
        /// Solo puede ser ejecutado por un administrador.
        /// </summary>
        /// <param name="id">ID del usuario a reactivar.</param>
        /// <returns>Datos del usuario reactivado.</returns>
        /// <response code="200">Usuario reactivado con éxito.</response>
        /// <response code="404">Usuario no encontrado.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id}/reactivate")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<UserDataDTO>> ReactivateUser([FromRoute] int id)
        {
            AppUser user = await this._userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            await this._userService.ReactivateUser(user);
            UserDataDTO userData = new UserDataDTO
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName,
            };
            return Ok(userData);
        }

        /// <summary>
        /// Desactiva un usuario.
        /// Administradores y el propio usuario pueden realizar esta operación.
        /// </summary>
        /// <param name="id">ID del usuario a desactivar.</param>
        /// <returns>Datos del usuario desactivado.</returns>
        /// <response code="200">Usuario desactivado con éxito.</response>
        /// <response code="404">Usuario no encontrado.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="403">Acceso denegado. Solo administradores pueden realizar esta operación.</response>
        [HttpPut("{id}/deactivate")]
        [Authorize(Policy = "AdminUser")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Código de estado 404: Not Found
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        [ProducesResponseType(StatusCodes.Status403Forbidden)] // Código de estado 403: Forbidden
        public async Task<ActionResult<UserDataDTO>> DeactivateUser([FromRoute] int id)
        {
            AppUser user = await this._userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            await this._userService.DeactivateUser(user);
            UserDataDTO userData = new UserDataDTO
            {
                Id = user.Id,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName,
            };
            return Ok(userData);
        }

        /// <summary>
        /// Cambia el rol de un usuario.
        /// Solo puede ser ejecutado por un administrador.
        /// </summary>
        /// <param name="id">ID del usuario a modificar.</param>
        /// <param name="role">Nuevo rol para el usuario.</param>
        /// <returns>Datos del usuario con su nuevo rol.</returns>
        /// <response code="200">Rol de usuario actualizado con éxito.</response>
        /// <response code="400">Error en la solicitud.</response>
        /// <response code="401">No autorizado.</response>
        [HttpPut("{id}/role")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)] // Código de estado 200: OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult<UserDataDTO>> ChangeUserRole([FromRoute] int id, [FromBody] EditUserRoleDTO role)
        {
            UserDataDTO userData = await this._userService.UpdateUserRole(id, role.RoleName);
            if (userData == null)
            {
                return BadRequest();
            }
            return Ok(userData);
        }

        /// <summary>
        /// Elimina un usuario de la aplicación.
        /// Solo puede ser ejecutado por un administrador.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>Estado de la operación de eliminación.</returns>
        /// <response code="204">Usuario eliminado con éxito.</response>
        /// <response code="400">Error al intentar eliminar el usuario.</response>
        /// <response code="401">No autorizado.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Código de estado 204: No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado 400: Bad Request
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Código de estado 401: Unauthorized
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            bool deletedUser = await this._userService.DeleteUser(id);
            if (!deletedUser)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
