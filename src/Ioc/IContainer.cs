using System;
using System.Collections;
using System.Collections.Generic;

namespace eQuantic.Core.Ioc;

/// <summary>
/// Defines a contract for an Inversion of Control (IoC) container that provides dependency resolution services.
/// </summary>
public interface IContainer
{
    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <returns>An instance of the specified type.</returns>
    T Resolve<T>();
    
    /// <summary>
    /// Resolves a named instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to resolve.</typeparam>
    /// <param name="name">The name of the registration to resolve.</param>
    /// <returns>A named instance of the specified type.</returns>
    T Resolve<T>(string name);
    
    /// <summary>
    /// Resolves an instance of the specified type.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>An instance of the specified type.</returns>
    object Resolve(Type type);
    
    /// <summary>
    /// Resolves a named instance of the specified type.
    /// </summary>
    /// <param name="name">The name of the registration to resolve.</param>
    /// <param name="type">The type to resolve.</param>
    /// <returns>A named instance of the specified type.</returns>
    object Resolve(string name, Type type);

    /// <summary>
    /// Resolves all registered instances of the specified type.
    /// </summary>
    /// <param name="type">The type to resolve all instances for.</param>
    /// <returns>An enumerable collection of all registered instances of the specified type.</returns>
    IEnumerable ResolveAll(Type type);
    
    /// <summary>
    /// Resolves all registered instances of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to resolve all instances for.</typeparam>
    /// <returns>An enumerable collection of all registered instances of the specified type.</returns>
    IEnumerable<T> ResolveAll<T>();
}