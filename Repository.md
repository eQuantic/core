# Repository Pattern with Entity Framework

## UnitOfWork example:

```csharp
using eQuantic.Core.Data.EntityFramework.Repository;
using eQuantic.Core.Ioc;
using Microsoft.EntityFrameworkCore;

namespace eQuantic.Core.Web.Examples.Infrastructure
{
    public class ExampleUnitOfWork : UnitOfWork
    {
        private readonly IContainer _container;

        public ExampleUnitOfWork(IContainer container, DbContext context) : base(context)
        {
            _container = container;
        }

        public override TRepository GetRepository<TRepository>()
        {
            return _container.Resolve<TRepository>();
        }

        public override TRepository GetRepository<TRepository>(string name)
        {
            return _container.Resolve<TRepository>(name);
        }
    }
}
```

## Entity data example:

```csharp
using System;
using eQuantic.Core.Data.Repository;

namespace eQuantic.Core.Web.Examples.Infrastructure.Data
{
    public class UserData : IEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}

namespace eQuantic.Core.Web.Examples.Infrastructure.Data
{
    public class PersonData : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public virtual UserData User { get; set; }
    }
}
```

## Repository example:
### Contract

```csharp
using System;
using eQuantic.Core.Data.Repository;
using eQuantic.Core.Web.Examples.Infrastructure.Data;

namespace eQuantic.Core.Web.Examples.Infrastructure.Repositories.Contracts
{
    public interface IPersonRepository : IAsyncRepository<ExampleUnitOfWork, PersonData, Guid>
    {
    }
}
```

### Implementation
```csharp
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
```

# DDD Pattern

## Domain Entity example:

```csharp
using System;

namespace eQuantic.Core.Web.Examples.Domain.Entities
{
    public class User
    {
        public ShortGuid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}

namespace eQuantic.Core.Web.Examples.Domain.Entities
{
    public class Person
    {
        public ShortGuid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public virtual User User { get; set; }
    }
}
```

## Specification Pattern

```csharp
using System;
using System.Linq.Expressions;
using eQuantic.Core.Web.Examples.Infrastructure.Data;
using eQuantic.Core.Linq.Specification;

namespace eQuantic.Core.Web.Examples.Domain.Specification
{
    public class PersonSpecification : Specification<PersonData>
    {
        private readonly string _term;

        public PersonSpecification(string term)
        {
            _term = term;
        }
        public override Expression<Func<PersonData, bool>> SatisfiedBy()
        {
            return p => p.Name.StartsWith(_term) || p.User.UserName.StartsWith(_term) || p.User.Email.StartsWith(_term);
        }
    }
}
```

## Domain Services

### Contract

```csharp
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
```

### Implementation
```csharp
using System;
using System.Collections.Generic;
using AutoMapper;
using eQuantic.Core.Collections;
using eQuantic.Core.Linq;
using eQuantic.Core.Web.Examples.Domain.Entities;
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
```
