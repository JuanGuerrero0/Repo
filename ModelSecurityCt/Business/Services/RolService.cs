using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RolService : ServiceBase<RolDTO, Rol>
    {
        public RolService(IRolRepository repository, ILogger<RolService> logger)
            : base(repository, logger) { }
    }
}
