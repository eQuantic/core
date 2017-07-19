using System;
using StructureMap;
using IContainer = eQuantic.Core.Ioc.IContainer;

namespace eQuantic.Core.Web.Examples.Ioc
{
    public class ExampleContainer : Container, IContainer
    {
        public T Resolve<T>()
        {
            return GetInstance<T>();
        }

        public T Resolve<T>(string name)
        {
            return GetInstance<T>(name);
        }

        public object Resolve(Type type)
        {
            return GetInstance(type);
        }

        public object Resolve(string name, Type type)
        {
            return GetInstance(type, name);
        }
    }
}
