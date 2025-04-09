using Business.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class PermissionService : ServiceBase<PermissionDTO, Permission>
    {
        public PermissionService(IPermissionRepository repository, ILogger<PermissionService> logger)
            : base(repository, logger) { }
    }
}
