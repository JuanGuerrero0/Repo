using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core
{
    /// <summary>
    /// Define el contrato base para los servicios de la capa de negocio.
    /// Utiliza DTOs para comunicarse con otras capas (por ejemplo, presentación o controladores),
    /// y entidades para interactuar con el repositorio.
    /// 
    /// Esta interfaz permite aplicar una arquitectura genérica y reutilizable,
    /// facilitando la separación de responsabilidades, testeo y mantenimiento.
    /// </summary>
    /// <typeparam name="TDto">Tipo de objeto de transferencia de datos (DTO).</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad del modelo de datos.</typeparam>
    public interface IServiceBase<TDto, TEntity>
    {/// <summary>
     /// Obtiene todos los registros en formato DTO.
     /// </summary>
     /// <returns>Una colección de objetos DTO.</returns>
        Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Obtiene un solo registro por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del registro.</param>
        /// <returns>El DTO correspondiente o una excepción si no se encuentra.</returns>
        Task<TDto> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo registro a partir de un DTO.
        /// </summary>
        /// <param name="dto">Objeto de transferencia con los datos a guardar.</param>
        /// <returns>El DTO creado con sus valores actualizados.</returns>
        Task<TDto> CreateAsync(TDto dto);

        /// <summary>
        /// Actualiza un registro existente a partir de un DTO.
        /// </summary>
        /// <param name="dto">Objeto de transferencia con los datos actualizados.</param>
        /// <returns>El DTO actualizado o una excepción si falla.</returns>
        Task<TDto> UpdateAsync(TDto dto);

        /// <summary>
        /// Elimina permanentemente un registro del sistema.
        /// </summary>
        /// <param name="id">Identificador del registro a eliminar.</param>
        /// <returns>True si se eliminó correctamente; false si no se encontró.</returns>
        Task<bool> DeletePermanentAsync(int id);

        /// <summary>
        /// Realiza una eliminación lógica del registro, si está soportada por la entidad.
        /// </summary>
        /// <param name="id">Identificador del registro a marcar como eliminado.</param>
        /// <returns>True si la operación fue exitosa; false en caso contrario.</returns>
        Task<bool> DeleteLogicalAsync(int id);
    }
}
