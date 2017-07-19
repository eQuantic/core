using System.Reflection;
using eQuantic.Core.Extensions;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace eQuantic.Core.Web.Examples.Ioc
{
    public class BaseTypeConvention : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed).ForEach(type =>
            {
                registry.For(type.GetTypeInfo().BaseType).Use(type).ContainerScoped();
            });
        }
    }
}
