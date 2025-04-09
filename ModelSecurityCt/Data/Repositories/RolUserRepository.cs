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
    public class RolUserRepository : GenericRepository<RolUser>, IRolUserRepository
    {
        public RolUserRepository(ApplicationDbContext context, ILogger<RolUserRepository> logger)
        : base(context, logger) { }
    }
}
