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
