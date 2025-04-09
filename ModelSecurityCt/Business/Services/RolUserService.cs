using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RolUserService : ServiceBase<RolUserDTO, RolUser>
    {
        public RolUserService(IRolUserRepository repository, ILogger<RolUserService> logger)
            : base(repository, logger) { }
    }
}
