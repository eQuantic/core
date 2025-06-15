using System;

namespace eQuantic.Core.Cache;

/// <summary>
/// Defines a contract for caching operations with support for generic types and expiration.
/// </summary>
public interface ICaching
{
    /// <summary>
    /// Adds an object to the cache using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the object to cache.</typeparam>
    /// <param name="object">The object to store in cache.</param>
    /// <param name="key">The unique key used to identify the cached item.</param>
    void Add<T>(T @object, string key);

    /// <summary>
    /// Adds an object to the cache using the specified key with a timeout.
    /// </summary>
    /// <typeparam name="T">The type of the object to cache.</typeparam>
    /// <param name="object">The object to store in cache.</param>
    /// <param name="key">The unique key used to identify the cached item.</param>
    /// <param name="timeout">The cache expiration timeout in minutes.</param>
    void Add<T>(T @object, string key, double timeout);

    /// <summary>
    /// Removes an item from the cache.
    /// </summary>
    /// <param name="key">The unique key of the item to remove.</param>
    void Remove(string key);

    /// <summary>
    /// Determines whether the specified key exists in the cache and is not null.
    /// </summary>
    /// <param name="key">The unique key to check.</param>
    /// <returns><c>true</c> if the key does not exist or the value is null; otherwise, <c>false</c>.</returns>
    bool IsNull(string key);

    /// <summary>
    /// Retrieves a strongly-typed object from the cache.
    /// </summary>
    /// <typeparam name="T">The expected type of the cached object.</typeparam>
    /// <param name="key">The unique key of the cached item.</param>
    /// <returns>The cached object of type <typeparamref name="T"/>, or the default value if not found.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Retrieves an object from the cache with the specified type.
    /// </summary>
    /// <param name="type">The expected type of the cached object.</param>
    /// <param name="key">The unique key of the cached item.</param>
    /// <returns>The cached object, or null if not found.</returns>
    object Get(Type type, string key);

    /// <summary>
    /// Gets all cache keys currently stored in the cache.
    /// </summary>
    /// <returns>An array of all cache keys.</returns>
    string[] AllKeys();
}