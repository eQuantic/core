using System;

namespace eQuantic.Core.Ioc
{
    public interface IContainer
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        object Resolve(Type type);
        object Resolve(string name, Type type);
    }
}
