using System;
using eQuantic.Core.Collections;
using eQuantic.Core.Web.Examples.Domain.Entities;

namespace eQuantic.Core.Web.Examples.Domain.Services.Contracts
{
    public interface IPersonService
    {
        Person Get(Guid id);
        bool Create(Person person);
        bool Update(Person person);
        bool Delete(Guid id);
        PagedList<Person> Find(string term, int pageIndex, int pageSize);
    }
}