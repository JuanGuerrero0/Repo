using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RolFormPermissionService : ServiceBase<RolFormPermissionDTO, RolFormPermission>
    {
        public RolFormPermissionService(IRolFormPermissionRepository repository, ILogger<RolFormPermissionService> logger)
            : base(repository, logger) { }
    }
}
