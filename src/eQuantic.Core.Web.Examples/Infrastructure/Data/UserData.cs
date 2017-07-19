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
