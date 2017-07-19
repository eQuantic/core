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
