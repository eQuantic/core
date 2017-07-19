using System;

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