using System;
using System.ComponentModel;
using System.Resources;

namespace eQuantic.Core.Attributes;

/// <summary>
/// Provides a localized description attribute that retrieves descriptions from resource files.
/// </summary>
public class LocalizedDescriptionAttribute : DescriptionAttribute
{
    private readonly string _resourceKey;
    private readonly ResourceManager _resource;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizedDescriptionAttribute"/> class.
    /// </summary>
    /// <param name="resourceKey">The resource key to look up in the resource manager.</param>
    /// <param name="resourceType">The type that contains the resource manager.</param>
    public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
    {
        _resource = new ResourceManager(resourceType);
        _resourceKey = resourceKey;
    }

    /// <summary>
    /// Gets the localized description from the resource manager using the specified resource key.
    /// </summary>
    /// <value>The localized description, or a formatted placeholder if the resource key is not found.</value>
    public override string Description
    {
        get
        {
            string displayName = _resource.GetString(_resourceKey);

            return string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", _resourceKey)
                : displayName;
        }
    }
}