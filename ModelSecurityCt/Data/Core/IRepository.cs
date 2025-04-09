using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.Core
{
    /// <summary>
    /// Define los métodos genéricos para acceder y manipular datos en el repositorio.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad sobre el cual se aplican las operaciones.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Obtiene todos los registros de la entidad T.
        /// Ideal para escenarios donde se necesita trabajar con listas completas.
        /// </summary>
        /// <returns>Una colección enumerable de entidades.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Obtiene un registro único por su identificador primario.
        /// Este método se apoya en la clave primaria definida en el modelo.
        /// </summary>
        /// <param name="id">Identificador único de la entidad.</param>
        /// <returns>Entidad encontrada o null si no existe.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Agrega una nueva entidad al contexto y la persiste en la base de datos.
        /// Puede ser extendido para asignar propiedades automáticas como CreatedDate.
        /// </summary>
        /// <param name="entity">Entidad a agregar.</param>
        /// <returns>Entidad persistida, con claves generadas si aplica.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Actualiza una entidad existente en la base de datos.
        /// Se asume que la entidad ya fue cargada previamente (tracking o attach).
        /// </summary>
        /// <param name="entity">Entidad con los datos actualizados.</param>
        /// <returns>True si la actualización fue exitosa.</returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Elimina físicamente una entidad según su identificador.
        /// Este método borra el registro permanentemente. Evitar si se requiere trazabilidad.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <returns>True si se eliminó exitosamente, False si no fue encontrada.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Realiza una eliminación lógica, útil para mantener historial o trazabilidad.
        /// La entidad debe implementar una propiedad llamada 'IsDeleted' de tipo boolean.
        /// </summary>
        /// <param name="id">Identificador de la entidad a marcar como eliminada.</param>
        /// <returns>True si se marcó como eliminada, False si no fue posible.</returns>
        Task<bool> DeleteLogicalAsync(int id);
    }



}
