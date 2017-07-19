using AutoMapper;
using eQuantic.Core.Web.Examples.Domain.Entities;
using eQuantic.Core.Web.Examples.Infrastructure.Data;

namespace eQuantic.Core.Web.Examples.Domain.Mappers
{
    public class PersonMapperProfile : Profile
    {
        public PersonMapperProfile()
        {
            CreateMap<PersonData, Person>()
                .ReverseMap();
        }
    }
}
