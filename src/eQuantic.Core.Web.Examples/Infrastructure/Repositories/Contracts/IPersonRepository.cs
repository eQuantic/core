using System;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Web.Examples.Infrastructure.Data;

namespace eQuantic.Core.Web.Examples.Infrastructure.Repositories.Contracts
{
    public interface IPersonRepository : IAsyncRepository<ExampleUnitOfWork, PersonData, Guid>
    {
    }
}