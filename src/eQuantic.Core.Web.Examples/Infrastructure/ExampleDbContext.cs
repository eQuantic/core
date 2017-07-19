using Microsoft.EntityFrameworkCore;

namespace eQuantic.Core.Web.Examples.Infrastructure
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
        {
            
        }
    }
}
