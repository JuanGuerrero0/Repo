using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModuleController : ControllerBase
    {
        private readonly ModuleService _moduleBusiness;
        private readonly ILogger<ModuleController> _logger;


        /// <summary>
        /// Constructor del controlador de users
        /// </summary>
        /// <param name="RolBusiness">Capa de negocio de users</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 
        public ModuleController(ModuleService _moduleBusiness, ILogger<ModuleController> _logger)
        {
            this._moduleBusiness = _moduleBusiness;
            this._logger = _logger;
        }

        /// <summary>
        /// Obtiene todos los Module del sistema
        /// </summary>
        /// <returns>Lista de Module</returns>
        /// <response code="200">Retorna la lista de Module</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllModule()
        {
            try
            {
                var Module = await _moduleBusiness.GetAllAsync();
                return Ok(Module);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los Module");
                return StatusCode(500, new { message = ex.Message });
            }
        }




        /// <summary>
        /// Obtiene un Module específico por su ID
        /// </summary>
        /// <param name="id">ID del Module</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el Module solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor<(/response>
        /// 

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetModuleById(int id)
        {
            try
            {

                var module = await _moduleBusiness.GetByIdAsync(id);
                return Ok(module);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el module con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "module no encontrado con ID: {module}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener user con ID: {module}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Crea un nuevo Module en el sistema
        /// </summary>
        /// <param name="RolDto">Datos del Module a crear</param>
        /// <returns>Module creado</returns>
        /// <response code="201">Retorna el Module creado</response>
        /// <response code="400">Datos del Module no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateModule([FromBody] ModuleDTO moduleDTO)
        {
            try
            {
                var createModule = await _moduleBusiness.CreateAsync(moduleDTO);
                return CreatedAtAction(nameof(GetModuleById), new
                {
                    id = createModule.Id
                }, createModule);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear el module");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza un module existente
        /// </summary>
        /// <param name="PermissionDTO">Datos del v a actualizar</param>
        /// <returns>module actualizado</returns>
        /// <response code="200">Retorna el module actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">module no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(ModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModule([FromBody] ModuleDTO moduleDTO)
        {
            try
            {
                if (moduleDTO == null || moduleDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del permiso debe ser mayor que cero y no nulo" });
                }

                var updateModule = await _moduleBusiness.UpdateAsync(moduleDTO);
                return Ok(updateModule);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el modulo");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "module no encontrado con ID: {RolId}", moduleDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el module con ID: {RolId}", moduleDTO.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }



        /// <summary>
        /// Elimina un module por su ID
        /// </summary>
        /// <param name="id">ID del module a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        /// <response code="200">module eliminado exitosamente</response>
        /// <response code="400">ID no válido</response>
        /// <response code="404">module no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteModule(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del module debe ser mayor que cero" });
                }

                await _moduleBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "module eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "module no encontrado con ID: {permission}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar el module con  ", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteModuleLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del module debe ser mayor que cero" });
                }

                await _moduleBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "module eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "module no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el module con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
