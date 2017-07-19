using System;
using eQuantic.Core.Data.Repository;

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
