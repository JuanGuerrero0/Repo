using Business;
using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FormController : ControllerBase
    {
        private readonly FormService _formBusiness;
        private readonly ILogger<FormController> logger;

        /// <summary>
        /// Constructor del controlador de users
        /// </summary>
        /// <param name="FormBusiness">Capa de negocio de Forms</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 
        public FormController(FormService _formBusiness, ILogger<FormController> logger)
        {
            this._formBusiness = _formBusiness;
            this.logger = logger;
        }

        /// <summary>
        /// Obtiene todos los Form del sistema
        /// </summary>
        /// <returns>Lista de Form</returns>
        /// <response code="200">Retorna la lista de Form</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var Form = await _formBusiness.GetAllAsync();
                return Ok(Form);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener los Form");
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
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {

                var form = await _formBusiness.GetByIdAsync(id);
                return Ok(form);

            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida para el form con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "form no encontrado con ID: {form}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al obtener form con ID: {module}", id);
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
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDTO FormDTO)
        {
            try
            {
                var createForm = await _formBusiness.CreateAsync(FormDTO);
                return CreatedAtAction(nameof(GetFormById), new
                {
                    id = createForm.Id
                }, createForm);

            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al crear el form");
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
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateModule([FromBody] FormDTO formDTO)
        {
            try
            {
                if (formDTO == null || formDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID del form debe ser mayor que cero y no nulo" });
                }

                var updateForm = await _formBusiness.UpdateAsync(formDTO);
                return Ok(updateForm);
            }
            catch (ValidationException ex)
            {
                logger.LogWarning(ex, "Validación fallida al actualizar el form");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "form no encontrado con ID: {RolId}", formDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al actualizar el form con ID: {RolId}", formDTO.Id);
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
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del form debe ser mayor que cero" });
                }

                await _formBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "module eliminado correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "form no encontrado con ID: {permission}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar el form con  ", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeleteFormLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID del form debe ser mayor que cero" });
                }

                await _formBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "form eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                logger.LogInformation(ex, "form no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                logger.LogError(ex, "Error al eliminar lógicamente  el form con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
