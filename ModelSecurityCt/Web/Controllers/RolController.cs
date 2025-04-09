using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{

    /// <summary>
    /// Controlador para la gestión de roles en el sistema
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class RolController : ControllerBase
    {
        private readonly RolService _rolBusiness;
        private readonly ILogger<RolController> _logger;

        /// <summary>
        /// Constructor del controlador de users
        /// </summary>
        /// <param name="RolBusiness">Capa de negocio de users</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 

        public RolController(RolService _rolBusiness, ILogger<RolController> _logger)
        {
            this._rolBusiness = _rolBusiness;
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
                var Rols = await _rolBusiness.GetAllAsync();
                return Ok(Rols);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Roles");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un user específico por su ID
        /// </summary>
        /// <param name="id">ID del user</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el user solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor<(/response>
        /// 

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolById(int id)
        {
            try
            {

                var Rol = await _rolBusiness.GetByIdAsync(id);
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


        /// <summary>
        /// Crea un nuevo user en el sistema
        /// </summary>
        /// <param name="RolDto">Datos del user a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el user creado</response>
        /// <response code="400">Datos del user no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRol([FromBody] RolDTO rolDTO)
        {
            try
            {
                var createRol = await _rolBusiness.CreateAsync(rolDTO);
                return CreatedAtAction(nameof(GetRolById), new
                {
                    id = createRol.Id
                }, createRol);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Rol");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }


        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        /// <param name="rolDTO">Datos del rol a actualizar</param>
        /// <returns>Rol actualizado</returns>
        /// <response code="200">Retorna el rol actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(RolDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRol([FromBody] RolDTO rolDTO)
        {
            try
            {
                if (rolDTO == null || rolDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del rol debe ser mayor que cero y no nulo" });
                }

                var updatedRol = await _rolBusiness.UpdateAsync(rolDTO);
                return Ok(updatedRol);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el rol");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", rolDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID: {RolId}", rolDTO.Id);
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
        public async Task<IActionResult> DeleteRol(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rol debe ser mayor que cero" });
                }

                await _rolBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "Rol eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteRolLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del rol debe ser mayor que cero" });
                }

                await _rolBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "Rol eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Rol no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el rol con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
