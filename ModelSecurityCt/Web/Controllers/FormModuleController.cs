using Entity.DTO.DTOUpdate;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using Business.Services;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormModuleController : ControllerBase
    {
        private readonly FormModuleService _formModuleBusiness;
        private readonly ILogger<FormModuleController> logger;


        /// <summary>
        /// Constructor del controlador de FormModule
        /// </summary>
        /// <param name="FormModulebusiness">Capa de negocio de FormModule</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 
        public FormModuleController(FormModuleService _formModuleBusiness, ILogger<FormModuleController> logger)
        {
            this._formModuleBusiness = _formModuleBusiness;
            this.logger = logger;
        }


        /// <summary>
        /// Obtiene todos los FormModule del sistema
        /// </summary>
        /// <returns>Lista de FormModule</returns>
        /// <response code="200">Retorna la lista de FormModule</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormModuleDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllFormModule()
        {
            try
            {
                var FormModule = await _formModuleBusiness.GetAllAsync();
                return Ok(FormModule);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener los FormModule");
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

                var formModule = await _formModuleBusiness.GetByIdAsync(id);
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
        [ProducesResponseType(typeof(FormModuleDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFormModule([FromBody] FormModuleDTO formModuleDTO)
        {
            try
            {
                var createFormModule = await _formModuleBusiness.CreateAsync(formModuleDTO);
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


        /// <summary>
        /// Actualiza un form existente
        /// </summary>
        /// <param name="formDTO">Datos del v a actualizar</param>
        /// <returns>form actualizado</returns>
        /// <response code="200">Retorna el form actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">form no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(FormModuleDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateFormModule([FromBody] FormModuleDTO FormModuleDTO)
        {
            try
            {
                if (FormModuleDTO == null || FormModuleDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del form debe ser mayor que cero y no nulo" });
                }

                var updateFormModule = await _formModuleBusiness.UpdateAsync(FormModuleDTO);
                return Ok(updateFormModule);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida al actualizar el FormModule");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "FormModule no encontrado con ID: {RolId}", FormModuleDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al actualizar el form con ID: {RolId}", FormModuleDTO.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Elimina un form por su ID
        /// </summary>
        /// <param name="id">ID del form a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        /// <response code="200">form eliminado exitosamente</response>
        /// <response code="400">ID no válido</response>
        /// <response code="404">form no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteFormModule(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del formmodule debe ser mayor que cero" });
                }

                await _formModuleBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "module eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "formmodule no encontrado con ID: {formmodule}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar el formmodule  ", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteFormModuleLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del formModule debe ser mayor que cero" });
                }

                await _formModuleBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "form eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "formModule no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar lógicamente  el formModule con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
