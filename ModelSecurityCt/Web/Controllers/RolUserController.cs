using Business.Services;
using Entity.DTO;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolUserController : ControllerBase
    {
        private readonly RolUserService _RolUserBusiness;
        private readonly ILogger<RolUserController> _Logger;

        public RolUserController(RolUserService rolUserBusiness, ILogger<RolUserController> logger)
        {
            _RolUserBusiness = rolUserBusiness;
            _Logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolUserDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolUsers()
        {
            try
            {
                var RolUsers = await _RolUserBusiness.GetAllAsync();
                return Ok(RolUsers);
            }
            catch (ExternalServiceException ex)
            {
                _Logger.LogError(ex, "Error al obtner RolForm");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolUserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolUserByIdAsync(int id)
        {
            try
            {
                var RolUser = await _RolUserBusiness.GetByIdAsync(id);
                return Ok(RolUser);
            }
            catch (ValidationException ex)
            {
                _Logger.LogWarning(ex, "Validación fallida por el RolUser con ID: {RolUserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _Logger.LogInformation(ex, "RolUser no encontrado con ID: {RolUser}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _Logger.LogError(ex, "Error al obtner RolUser con ID: {RolUserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(RolUserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        [HttpPost]
        public async Task<IActionResult> CreateRolUser([FromBody] RolUserDTO rolUserDto)
        {
            if (rolUserDto == null)
                return BadRequest(new { message = "El objeto RolUser no puede ser nulo." });

            try
            {
                var newRolUser = await _RolUserBusiness.CreateAsync(rolUserDto);
                return Ok(newRolUser);
            }
            catch (ValidationException ex)
            {
                _Logger.LogWarning(ex, "Error de validación al crear un RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _Logger.LogError(ex, "Error en la base de datos al crear un RolUser");
                return StatusCode(500, new { message = "Error interno del servidor al procesar la solicitud." });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error desconocido al crear un RolUser");
                return StatusCode(500, new { message = "Ocurrió un error inesperado." });
            }
        }



        [HttpPut]
        [ProducesResponseType(typeof(RolUserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateRolUser([FromBody] RolUserDTO RolUserDto)
        {


            try
            {
                var updatedForm = await _RolUserBusiness.UpdateAsync(RolUserDto);
                return Ok(updatedForm);
            }
            catch (ValidationException ex)
            {
                _Logger.LogWarning(ex, "Validación fallida al actualizar RolUser");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _Logger.LogInformation(ex, "RolUser no encontrado con ID {RolUserId}");
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _Logger.LogError(ex, "Error al RolUser Form con ID {RolUserId}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteLogic(int id)
        {
            try
            {
                var success = await _RolUserBusiness.DeletePermanentAsync(id);
                if (!success)
                    return NotFound(new { message = "RolUser no encontrado o ya inactivo" });

                return Ok(new { message = "RolUser eliminado lógicamente" });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error al eliminar lógicamente el RolUser con ID {RolUserid}", id);
                return StatusCode(500, new { message = "Error interno al eliminar el RolUser" });
            }
        }

        [HttpDelete("persistent/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePersistent(int id)
        {
            try
            {
                var success = await _RolUserBusiness.DeleteLogicalAsync(id);
                if (!success)
                    return NotFound(new { message = "RolUser no encontrado o ya eliminado" });

                return Ok(new { message = "RolUser eliminado permanentemente" });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Error al eliminar permanentemente el RolUser con ID {id}", id);
                return StatusCode(500, new { message = "Error interno al eliminar el RolUser" });
            }
        }

    }
}
