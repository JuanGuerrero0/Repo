using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userBusiness;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService _userBusiness, ILogger<UserController> _logger)
        {
            this._userBusiness = _userBusiness;
            this._logger = _logger;
        }
        /// <summary>
        /// Obtiene todos los users del sistema
        /// </summary>
        /// <returns>Lista de users</returns>
        /// <response code="200">Retorna la lista de users</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRols()
        {
            try
            {
                var Rols = await _userBusiness.GetAllAsync();
                return Ok(Rols);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Roles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {

                var Rol = await _userBusiness.GetByIdAsync(id);
                return Ok(Rol);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el user con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener user con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFormModule([FromBody] UserDTO UserDTO)
        {
            try
            {
                var createFormModule = await _userBusiness.CreateAsync(UserDTO);
                return CreatedAtAction(nameof(GetRolById), new
                {
                    id = createFormModule.Id
                }, createFormModule);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el formModule");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRol([FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO == null || userDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del user debe ser mayor que cero y no nulo" });
                }

                var updateUSer = await _userBusiness.UpdateAsync(userDTO);
                return Ok(updateUSer);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el user");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "user no encontrado con ID: {user}", userDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el user con ID:", userDTO.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Elimina un rol por su ID
        /// </summary>
        /// <param name="id">ID del rol a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        /// <response code="200">Rol eliminado exitosamente</response>
        /// <response code="400">ID no válido</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del user debe ser mayor que cero" });
                }

                await _userBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "user eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "user no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el user con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteUserLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del user debe ser mayor que cero" });
                }

                await _userBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "user eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "user no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el user con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
