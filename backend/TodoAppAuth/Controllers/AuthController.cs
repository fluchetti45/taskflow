using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppAuth.Dtos;
using TodoAppAuth.Models;
using TodoAppAuth.Services;

namespace TodoAppAuth.Controllers
{
    /// <summary>
    /// Controlador para manejar las operaciones de autenticación y registro de usuarios.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        /// <param name="authService">Servicio de autenticación para manejar las operaciones de login y signup.</param>
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión en el sistema.
        /// </summary>
        /// <param name="userData">Objeto que contiene el correo electrónico y la contraseña del usuario.</param>
        /// <returns>Un token de acceso si las credenciales son válidas, o un mensaje de error si no lo son.</returns>
        /// <response code="200">Login exitoso. Devuelve el token de acceso.</response>
        /// <response code="400">Credenciales incorrectas o cuenta desactivada.</response>
        [HttpPost("login")]
        [AllowAnonymous]  // Permite el acceso sin autenticación previa
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(LoginUserDTO userData)
        {
            // Intenta autenticar al usuario con las credenciales proporcionadas
            AppUser user = await this._authService.LoginUser(userData);
            if (user == null)
            {
                return BadRequest(new { message = "Credenciales incorrectas." });
            }
            if (user.IsActive == false)
            {
                return BadRequest(new { message = "Tu cuenta fue desactivada. Contactate con soporte para darla de alta." });
            }

            // Si el usuario está activo, genera un token de acceso
            var token = _authService.GenerateToken(user.Id, user.Email, user.Role.RoleName);
            return Ok(new { Token = token });
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userData">Objeto que contiene los datos del nuevo usuario (como correo, contraseña, etc.).</param>
        /// <returns>Un código de estado HTTP indicando el resultado de la operación.</returns>
        /// <response code="201">Usuario registrado exitosamente.</response>
        /// <response code="400">Error al intentar registrar al usuario.</response>
        [HttpPost("signup")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)] // Código de estado para éxito en la creación
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado para error en la validación
        public async Task<ActionResult> Signup(RegisterUserDTO userData)
        {
            try
            {
                AppUser createdUser = await this._authService.SignupUser(userData);
                return Created("", new { Message = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        /// <summary>
        /// Registra un nuevo usuario con rol de administrador. Solo accesible para administradores.
        /// </summary>
        /// <param name="userData">Objeto que contiene los datos del nuevo usuario (como correo, contraseña, etc.).</param>
        /// <returns>Un código de estado HTTP indicando el resultado de la operación.</returns>
        /// <response code="201">Usuario administrador registrado exitosamente.</response>
        /// <response code="400">Error al intentar registrar al usuario administrador.</response>
        [HttpPost("signup/admin")]
        [Authorize(Policy = "AdminOnly")]  // Solo accesible para usuarios con rol de administrador
        [ProducesResponseType(StatusCodes.Status201Created)] // Código de estado para éxito en la creación
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Código de estado para error en la validación
        public async Task<ActionResult> SignupAdmin(RegisterUserDTO userData)
        {
            // Intenta registrar al nuevo usuario como administrador
            AppUser createdUser = await this._authService.SignupUser(userData, roleName: "Admin");
            if (createdUser == null)
            {
                return BadRequest();
            }

            // Si el usuario administrador fue creado, devuelve un código 201
            return Created();
        }
    }
}
