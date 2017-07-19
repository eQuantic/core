using System;
using System.Collections.Generic;
using AutoMapper;
using eQuantic.Core.Collections;
using eQuantic.Core.Linq;
using eQuantic.Core.Web.Examples.Domain.Entities;
using eQuantic.Core.Web.Examples.Domain.Services.Contracts;
using eQuantic.Core.Web.Examples.Domain.Specification;
using eQuantic.Core.Web.Examples.Infrastructure;
using eQuantic.Core.Web.Examples.Infrastructure.Data;
using eQuantic.Core.Web.Examples.Infrastructure.Repositories.Contracts;

namespace eQuantic.Core.Web.Examples.Domain.Services
{
    public class PersonService : IPersonService
    {
        public IMapper Mapper { get; }
        public ExampleUnitOfWork UnitOfWork { get; }

        public PersonService(IMapper mapper, ExampleUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        public Person Get(Guid id)
        {
            Person person = null;
            var repo = UnitOfWork.GetRepository<IPersonRepository>();
            var item = repo.Get(id, p => p.User);
            if (item != null) person = Mapper.Map<Person>(item);

            return person;
        }

        public bool Create(Person person)
        {
            var repo = UnitOfWork.GetRepository<IPersonRepository>();
            var item = Mapper.Map<PersonData>(person);
            repo.Add(item);
            return UnitOfWork.Commit() > 0;
        }

        public bool Update(Person person)
        {
            var repo = UnitOfWork.GetRepository<IPersonRepository>();
            var item = repo.Get(person.Id);
            Mapper.Map(person, item);
            repo.Modify(item);
            return UnitOfWork.Commit() > 0;
        }

        public bool Delete(Guid id)
        {
            var repo = UnitOfWork.GetRepository<IPersonRepository>();
            var item = repo.Get(id);
            repo.Remove(item);
            return UnitOfWork.Commit() > 0;
        }

        public PagedList<Person> Find(string term, int pageIndex, int pageSize)
        {
            var repo = UnitOfWork.GetRepository<IPersonRepository>();
            var specification = new PersonSpecification(term);
            var count = repo.Count(specification);
            var items = repo.GetPaged(specification, pageIndex, pageSize,
                new[] {new Sorting<PersonData> {Column = c => c.Name}}, p => p.User);
            var persons = Mapper.Map<IEnumerable<Person>>(items);
            return new PagedList<Person>(persons, count){ PageIndex = pageIndex, PageSize = pageSize};
        }
    }
}
