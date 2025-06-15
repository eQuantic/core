using NUnit.Framework;
using eQuantic.Core.Extensions;
using System;

namespace eQuantic.Core.Tests.Extensions;

[TestFixture]
public class GuidExtensionsTests
{
    [Test]
    public void ToShort_WithValidGuid_ReturnsShortGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = guid.ToShort();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToGuid(), Is.EqualTo(guid));
    }

    [Test]
    public void ToShort_WithEmptyGuid_ReturnsEmptyShortGuid()
    {
        // Arrange
        var guid = Guid.Empty;

        // Act
        var result = guid.ToShort();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ToGuid(), Is.EqualTo(Guid.Empty));
        Assert.That(result, Is.EqualTo(ShortGuid.Empty));
    }

    [Test]
    public void ToShort_WithSpecificGuid_ReturnsConsistentShortGuid()
    {
        // Arrange
        var guid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var result1 = guid.ToShort();
        var result2 = guid.ToShort();

        // Assert
        Assert.That(result1.ToString(), Is.EqualTo(result2.ToString()));
        Assert.That(result1.ToGuid(), Is.EqualTo(result2.ToGuid()));
    }

    [Test]
    public void ToShort_RoundTrip_PreservesOriginalGuid()
    {
        // Arrange
        var originalGuid = Guid.NewGuid();

        // Act
        var shortGuid = originalGuid.ToShort();
        var convertedBackGuid = shortGuid.ToGuid();

        // Assert
        Assert.That(convertedBackGuid, Is.EqualTo(originalGuid));
    }
} 