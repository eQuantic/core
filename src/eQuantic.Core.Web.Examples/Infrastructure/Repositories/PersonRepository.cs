using System;
using eQuantic.Core.Data.EntityFramework.Repository;
using eQuantic.Core.Web.Examples.Infrastructure.Data;
using eQuantic.Core.Web.Examples.Infrastructure.Repositories.Contracts;

namespace eQuantic.Core.Web.Examples.Infrastructure.Repositories
{
    public class PersonRepository : AsyncRepository<ExampleUnitOfWork, PersonData, Guid>, IPersonRepository
    {
        public PersonRepository(ExampleUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}