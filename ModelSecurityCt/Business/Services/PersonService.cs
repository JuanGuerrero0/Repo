using Business.Core;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class PersonService : ServiceBase<PersonDTO, Person>
    {
        public PersonService(IPersonRepository repository, ILogger<PersonService> logger)
            : base(repository, logger) { }
    }
}
