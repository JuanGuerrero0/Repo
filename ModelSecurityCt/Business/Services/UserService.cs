using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>
    {
        public UserService(IUserRepository repository, ILogger<UserService> logger)
            : base(repository, logger) { }
    }
}
