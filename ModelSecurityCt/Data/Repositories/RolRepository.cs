using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class RolRepository : GenericRepository<Rol>,IRolRepository
    {
        public RolRepository(ApplicationDbContext context, ILogger<RolRepository> logger)
        : base(context, logger) { }
    }
}
