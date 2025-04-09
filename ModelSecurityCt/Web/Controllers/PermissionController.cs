using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {

        private readonly PermissionService _permissionBusiness;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController(PermissionService _permissionBusiness, ILogger<PermissionController> _logger)
        {
            this._permissionBusiness = _permissionBusiness;
            this._logger = _logger;

        }

        /// <summary>
        /// Constructor del controlador de users
        /// </summary>
        /// <param name="RolBusiness">Capa de negocio de users</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 



        /// <summary>
        /// Obtiene todos los users del sistema
        /// </summary>
        /// <returns>Lista de users</returns>
        /// <response code="200">Retorna la lista de users</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPermission()
        {
            try
            {
                var Permission = await _permissionBusiness.GetAllAsync();
                return Ok(Permission);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Permisos");
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
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {

                var Permission = await _permissionBusiness.GetByIdAsync(id);
                return Ok(Permission);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el Permission con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RoPermissionlId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener user con ID: {Permission}", id);
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
        [ProducesResponseType(typeof(PermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDTO permissionDTO)
        {
            try
            {
                var createPermission = await _permissionBusiness.CreateAsync(permissionDTO);
                return CreatedAtAction(nameof(GetPermissionById), new
                {
                    id = createPermission.Id
                }, createPermission);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el Permiso");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }



        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        /// <param name="PermissionDTO">Datos del rol a actualizar</param>
        /// <returns>Rol actualizado</returns>
        /// <response code="200">Retorna el rol actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">Rol no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(PermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionDTO permissionDTO)
        {
            try
            {
                if (permissionDTO == null || permissionDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del permiso debe ser mayor que cero y no nulo" });
                }

                var updatePermission = await _permissionBusiness.UpdateAsync(permissionDTO);
                return Ok(updatePermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "permiso no encontrado con ID: {RolId}", permissionDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el permiso con ID: {RolId}", permissionDTO.Id);
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
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del permission debe ser mayor que cero" });
                }

                await _permissionBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "permission eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "permission no encontrado con ID: {permission}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el permission con  ", id);
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
                    return BadRequest(new { message = "El ID del permiso debe ser mayor que cero" });
                }

                await _permissionBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "permiso eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "permiso no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el permiso con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }






    }
}
