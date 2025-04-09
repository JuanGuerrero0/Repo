using Business.Core;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class FormModuleService : ServiceBase<FormModuleDTO, FormModule>
    {
        public FormModuleService(IFormModuleRepository repository, ILogger<FormModuleService> logger)
            : base(repository, logger) { }

    }
}
