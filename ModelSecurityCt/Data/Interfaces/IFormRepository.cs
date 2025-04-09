using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Entity.Model;

namespace Data.Interfaces
{/// <summary>
 /// Repositorio específico para la entidad Form.
 /// Extiende la funcionalidad genérica del IRepository con la posibilidad
 /// de incluir métodos personalizados relacionados con formularios.
 /// 
 /// Mantener interfaces específicas por entidad facilita la extensión del repositorio
 /// con operaciones que no aplican a otras entidades (por ejemplo, GetFormsByUserId).
 /// </summary>
    public interface IFormRepository : IRepository<Form>
    {
        // Aquí podrían agregarse métodos personalizados, como:
        // Task<IEnumerable<Form>> GetFormsByUserIdAsync(int userId);
        // Task<Form> GetLatestSubmittedFormAsync();
    }
}
