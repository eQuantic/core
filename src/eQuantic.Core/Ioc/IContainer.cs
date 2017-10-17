using System;
using System.Collections;
using System.Collections.Generic;

namespace eQuantic.Core.Ioc
{
    public interface IContainer
    {
        T Resolve<T>();
        T Resolve<T>(string name);
        object Resolve(Type type);
        object Resolve(string name, Type type);

        IEnumerable ResolveAll(Type type);
        IEnumerable<T> ResolveAll<T>();
    }
}
