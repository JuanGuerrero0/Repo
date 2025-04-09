using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFormPermissionController : ControllerBase
    {
        private readonly RolFormPermissionService _rolFormPermissionBusiness;
        private readonly ILogger<RolFormPermissionController> logger;

        public RolFormPermissionController(RolFormPermissionService _rolFormPermissionBusiness, ILogger<RolFormPermissionController> logger)
        {
            this._rolFormPermissionBusiness = _rolFormPermissionBusiness;
            this.logger = logger;

        }



        /// <summary>
        /// Obtiene todos los Personas del sistema
        /// </summary>
        /// <returns>Lista de Personas</returns>
        /// <response code="200">Retorna la lista de Personas</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFormPermissionDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolFormPermission()
        {
            try
            {
                var Persons = await _rolFormPermissionBusiness.GetAllAsync();
                return Ok(Persons);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las RolFormPermission");
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Obtiene un Form específico por su ID
        /// </summary>
        /// <param name="id">ID del Form</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el Form solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor<(/response>
        /// 

        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormModuleById(int id)
        {
            try
            {

                var formModule = await _rolFormPermissionBusiness.GetByIdAsync(id);
                return Ok(formModule);

            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida para el formModule con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "formModule no encontrado con ID: {form}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al obtener formModule con ID: {module}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }



        /// <summary>
        /// Crea un nuevo form en el sistema
        /// </summary>
        /// <param name="FormDTO">Datos del form a crear</param>
        /// <returns>Module creado</returns>
        /// <response code="201">Retorna el form creado</response>
        /// <response code="400">Datos del form no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolFormPermissionDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFormModule([FromBody] RolFormPermissionDTO rolFormPermissionDTO)
        {
            try
            {
                var createFormModule = await _rolFormPermissionBusiness.CreateAsync(rolFormPermissionDTO);
                return CreatedAtAction(nameof(GetFormModuleById), new
                {
                    id = createFormModule.Id
                }, createFormModule);

            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al crear el formModule");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }



        [HttpPut]
        [ProducesResponseType(typeof(RolFormPermissionDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolUser([FromBody] RolFormPermissionDTO rolFormPermissionDTO)
        {


            try
            {
                var updatedForm = await _rolFormPermissionBusiness.UpdateAsync(rolFormPermissionDTO);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida al actualizar RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "RolUser no encontrado con ID {RolUserId}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al RolUser Form con ID {RolUserId}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteRolFormPermission(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID de la persona  debe ser mayor que cero" });
                }

                await _rolFormPermissionBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "persona eliminada correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "Persona no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar la persona con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteRolFormPermissionLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID de la RolFormPermission debe ser mayor que cero" });
                }

                await _rolFormPermissionBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "RolFormPermission eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "RolFormPermission no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar lógicamente  el RolFormPermission con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
