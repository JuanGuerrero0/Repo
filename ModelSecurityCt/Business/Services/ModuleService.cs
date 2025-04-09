using Business.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class ModuleService : ServiceBase<ModuleDTO, Module>
    {
        public ModuleService(IModuleRepository repository, ILogger<ModuleService> logger)
            : base(repository, logger) { }
    }
}
