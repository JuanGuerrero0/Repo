using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core;
using Entity.DTO;
using Entity.Model;

namespace Business.Interfaces
{
    /// <summary>
    /// Contrato de servicio específico para la entidad Person.
    /// Hereda de IServiceBase y puede ser extendido con métodos propios de la lógica de negocio de personas.
    /// </summary>
    public interface IPersonService : IServiceBase<PersonDTO,Person>
    {
        // Aquí se pueden agregar métodos específicos del negocio relacionados a Person,
        // como: BuscarPorDocumento, ObtenerFamiliares, ValidarMayorEdad, etc.
    }
}
