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
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personBusiness;
        private readonly ILogger<PersonController> _logger;

        /// <summary>
        /// Constructor del controlador de Personas
        /// </summary>
        /// <param name="PersonBusiness">Capa de negocio de Personas</param>
        /// <param name="logger">Logger para registro de eventos</param>
        /// 
        public PersonController(PersonService _personBusiness, ILogger<PersonController> _logger)
        {
            this._personBusiness = _personBusiness;
            this._logger = _logger;
        }


        /// <summary>
        /// Obtiene todos los Personas del sistema
        /// </summary>
        /// <returns>Lista de Personas</returns>
        /// <response code="200">Retorna la lista de Personas</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                var Persons = await _personBusiness.GetAllAsync();
                return Ok(Persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las personas");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un persona específico por su ID
        /// </summary>
        /// <param name="id">ID del persona</param>
        /// <returns>persona solicitado</returns>
        /// <response code="200">Retorna el persona solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">persona no encontrado</response>
        /// <response code="500">Error interno del servidor<(/response>
        /// 


        [HttpGet("{id:int}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {

                var person = await _personBusiness.GetByIdAsync(id);
                return Ok(person);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el person con ID:" + id);
                return BadRequest(new { Mesagge = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrado con ID: {personId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener person con ID: {personId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo person en el sistema
        /// </summary>
        /// <param name="personDTO">Datos del person a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el person creado</response>
        /// <response code="400">Datos del person no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        /// 

        [HttpPost]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO personDTO)
        {
            try
            {
                var createPerson = await _personBusiness.CreateAsync(personDTO);
                return CreatedAtAction(nameof(GetPersonById), new
                {
                    id = createPerson.Id
                }, createPerson);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida");
                return BadRequest(new { mesagge = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear la persona");
                return StatusCode(500, new { mesagge = ex.Message });
            }
        }



        /// <summary>
        /// Actualiza un persona existente
        /// </summary>
        /// <param name="personaDTO">Datos del persona a actualizar</param>
        /// <returns>persona actualizado</returns>
        /// <response code="200">Retorna el persona actualizado</response>
        /// <response code="400">Datos no válidos</response>
        /// <response code="404">persona no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPut]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonDTO personDTO)
        {
            try
            {
                if (personDTO == null || personDTO.Id <= 0)
                {
                    return BadRequest(new { message = "El ID de la persona  debe ser mayor que cero y no nulo" });
                }

                var updatePerson = await _personBusiness.UpdateAsync(personDTO);
                return Ok(updatePerson);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al actualizar el persona");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "persona no encontrado con ID: {RolId}", personDTO.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar el persona con ID co: {RolId}", personDTO.Id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Elimina un persona por su ID
        /// </summary>
        /// <param name="id">ID del persona a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        /// <response code="200">persona eliminado exitosamente</response>
        /// <response code="400">ID no válido</response>
        /// <response code="404">persona no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        /// 
        [HttpDelete("permanent/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID de la persona  debe ser mayor que cero" });
                }

                await _personBusiness.DeletePermanentAsync(id);
                return Ok(new { message = "persona eliminada correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Persona no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar la persona con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("Logico/{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public async Task<IActionResult> DeletePersonLogical(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El ID de la persona debe ser mayor que cero" });
                }

                await _personBusiness.DeleteLogicalAsync(id);
                return Ok(new { message = "persona eliminado lógico correctamente" });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "persona no encontrado con ID: " + id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar lógicamente  el persona con ID:" + id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
