# eQuantic Core Library

[![NuGet](https://img.shields.io/nuget/v/eQuantic.Core.svg)](https://www.nuget.org/packages/eQuantic.Core/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/eQuantic.Core.svg)](https://www.nuget.org/packages/eQuantic.Core/)
[![License](https://img.shields.io/github/license/eQuantic/core.svg)](https://github.com/eQuantic/core/blob/master/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.1%20%7C%20net6.0%20%7C%20net7.0%20%7C%20net8.0-blue.svg)](https://github.com/eQuantic/core)

**eQuantic Core** is a comprehensive .NET library that provides a rich set of utilities, extensions, and design pattern implementations to enhance your applications. This library serves as the foundation for the entire eQuantic ecosystem, offering essential functionality that supports various other eQuantic packages.

## 🚀 Features

### 📦 Collections

- **PagedList<T>**: Generic paged collection implementation
- **IPagedEnumerable**: Interface for paged enumerations with metadata (PageIndex, PageSize, TotalCount)

### 🧩 Extensions

Comprehensive extension methods for common .NET types:

- **StringExtensions**: Advanced string manipulation (LeftOf, RightOf, RemoveDiacritics, Slugify, etc.)
- **TaskExtensions**: Async/await utilities and task management
- **TypeExtensions**: Reflection helpers and type utilities
- **EnumerableExtensions**: LINQ extensions and collection utilities
- **DateTimeExtensions**: Date and time manipulation helpers
- **ExceptionExtensions**: Exception handling utilities
- **GuidExtensions**: GUID manipulation helpers
- **ListExtensions**: List-specific extension methods
- **EnumExtensions**: Enumeration utilities and helpers

### 📅 Date & Time Utilities

Advanced date and time manipulation tools:

- **TimeTool**: Comprehensive time calculations and manipulations
- **TimeFormatter**: Flexible time formatting utilities
- **DateDiff**: Date difference calculations
- **SystemClock**: Clock abstraction for testability
- **Time**: Advanced time operations
- **TimeCompare**: Time comparison utilities

### 🔒 Security

- **IEncrypting**: Encryption interface with encoding capabilities
- **Encryptor**: Default encryption implementation

### 🏗️ Design Patterns

- **IContainer**: IoC container interface for dependency injection
- **ICaching**: Caching interface with generic support

### 🛠️ Utilities

- **ShortGuid**: Base64-encoded GUID implementation for shorter string representations
- **RegNumberValidation**: Registration number validation utilities
- **MethodExtractorVisitor**: Expression tree method extraction

### 🏷️ Attributes & Constants

- **LocalizedDescriptionAttribute**: Localized description attribute for enums
- **StringConstants**: Common string constants

### 📡 Media Formatters

- **DateTimeConverterISO8601**: ISO 8601 DateTime converter
- **HttpFile**: HTTP file handling utilities

## 📥 Installation

Install **eQuantic.Core** via NuGet Package Manager:

```bash
dotnet add package eQuantic.Core
```

Or via Package Manager Console:

```powershell
Install-Package eQuantic.Core
```

## 🎯 Usage Examples

### Collections - Paged Results

```csharp
using eQuantic.Core.Collections;

// Create a paged list
var items = new List<string> { "item1", "item2", "item3" };
var pagedList = new PagedList<string>(items, totalCount: 100)
{
    PageIndex = 1,
    PageSize = 10
};

Console.WriteLine($"Page {pagedList.PageIndex} of {Math.Ceiling((double)pagedList.TotalCount / pagedList.PageSize)}");
```

### String Extensions

```csharp
using eQuantic.Core.Extensions;

string text = "Hello World";
string left = text.LeftOf(' '); // "Hello"
string right = text.RightOf(' '); // "World"

string withAccents = "Olá, mão!";
string slugified = withAccents.Slugify(); // "ola-mao"

string dirty = "Remove<script>alert('xss')</script>text";
string clean = dirty.UnHtml(); // "Removetext"
```

### ShortGuid Usage

```csharp
using eQuantic.Core;

// Generate a new short GUID (22 characters instead of 36)
ShortGuid shortId = ShortGuid.NewGuid();
Console.WriteLine(shortId.ToString()); // e.g., "FEx1sZbaCUifqz7VAp3j_g"

// Convert from regular GUID
Guid regularGuid = Guid.NewGuid();
ShortGuid fromGuid = new ShortGuid(regularGuid);

// Convert back to GUID
Guid converted = fromGuid.ToGuid();
```

### Date & Time Utilities

```csharp
using eQuantic.Core.Date;

// Advanced date operations
DateTime date = DateTime.Now;
DateTime startOfWeek = TimeTool.GetStartOfWeek(date, DayOfWeek.Monday);
DateTime dateOnly = TimeTool.GetDate(date);
DateTime withNewTime = TimeTool.SetTimeOfDay(date, 14, 30, 0); // 2:30 PM

// Date difference calculations
DateDiff diff = new DateDiff(DateTime.Now.AddDays(-30), DateTime.Now);
Console.WriteLine($"Difference: {diff.Days} days");
```

### Type Extensions & Reflection

```csharp
using eQuantic.Core.Extensions;

Type stringType = typeof(string);
bool isNullable = stringType.IsNullable(); // false

Type nullableInt = typeof(int?);
bool isNullableInt = nullableInt.IsNullable(); // true

// Get all types implementing an interface
var implementingTypes = typeof(IDisposable).GetImplementingTypes();
```

### Caching Interface

```csharp
using eQuantic.Core.Cache;

public class MyService
{
    private readonly ICaching _cache;

    public MyService(ICaching cache)
    {
        _cache = cache;
    }

    public async Task<string> GetDataAsync(string key)
    {
        if (!_cache.IsNull(key))
        {
            return _cache.Get<string>(key);
        }

        var data = await FetchDataFromSourceAsync(key);
        _cache.Add(data, key, timeout: 60); // Cache for 60 minutes
        return data;
    }
}
```

## 🎯 Target Frameworks

- .NET Standard 2.1
- .NET 6.0
- .NET 7.0
- .NET 8.0

## 🤝 Contributing

We welcome contributions! Please feel free to submit pull requests or open issues to discuss potential improvements.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Related Packages

eQuantic Core is part of the eQuantic ecosystem. Check out these related packages:

- [eQuantic.Core.Data](https://github.com/eQuantic/core-data) - Data access patterns and repositories
- [eQuantic.Core.Data.EntityFramework](https://github.com/eQuantic/core-data-entityframework) - Entity Framework implementation

## 📞 Support

For support, please visit our [GitHub Issues](https://github.com/eQuantic/core/issues) page.

---

**eQuantic Systems** - Building the future, one component at a time.
