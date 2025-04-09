using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO.DTOUpdate
{
    public class UpdateFormModuleDTO
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int ModuleId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
