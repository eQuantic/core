using NUnit.Framework;
using System;

namespace eQuantic.Core.Tests;

[TestFixture]
public class ShortGuidTests
{
    [Test]
    public void Constructor_WithValidGuid_CreatesShortGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var shortGuid = new ShortGuid(guid);

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(guid));
    }

    [Test]
    public void Constructor_WithValidString_CreatesShortGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuidString = ShortGuid.Encode(guid);

        // Act
        var shortGuid = new ShortGuid(shortGuidString);

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(guid));
    }

    [Test]
    public void NewGuid_CreatesNewShortGuid()
    {
        // Act
        var shortGuid1 = ShortGuid.NewGuid();
        var shortGuid2 = ShortGuid.NewGuid();

        // Assert
        Assert.That(shortGuid1, Is.Not.EqualTo(shortGuid2));
        Assert.That(shortGuid1.ToGuid(), Is.Not.EqualTo(Guid.Empty));
        Assert.That(shortGuid2.ToGuid(), Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void Encode_WithValidGuid_ReturnsBase64String()
    {
        // Arrange
        var guid = new Guid("12345678-1234-1234-1234-123456789012");

        // Act
        var encoded = ShortGuid.Encode(guid);

        // Assert
        Assert.That(encoded, Is.Not.Null);
        Assert.That(encoded.Length, Is.EqualTo(22));
        Assert.That(encoded, Does.Not.Contain("/"));
        Assert.That(encoded, Does.Not.Contain("+"));
    }

    [Test]
    public void Decode_WithValidString_ReturnsGuid()
    {
        // Arrange
        var originalGuid = Guid.NewGuid();
        var encoded = ShortGuid.Encode(originalGuid);

        // Act
        var decoded = ShortGuid.Decode(encoded);

        // Assert
        Assert.That(decoded, Is.EqualTo(originalGuid));
    }

    [Test]
    public void ToString_ReturnsEncodedString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuid = new ShortGuid(guid);
        var expectedString = ShortGuid.Encode(guid);

        // Act
        var result = shortGuid.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expectedString));
    }

    [Test]
    public void SetGuid_WithNewGuid_UpdatesValue()
    {
        // Arrange
        var shortGuid = new ShortGuid(Guid.NewGuid());
        var newGuid = Guid.NewGuid();

        // Act
        shortGuid.SetGuid(newGuid);

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(newGuid));
    }

    [Test]
    public void SetValue_WithNewString_UpdatesGuid()
    {
        // Arrange
        var shortGuid = new ShortGuid(Guid.NewGuid());
        var newGuid = Guid.NewGuid();
        var newValue = ShortGuid.Encode(newGuid);

        // Act
        shortGuid.SetValue(newValue);

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(newGuid));
    }

    [Test]
    public void Equals_WithSameGuid_ReturnsTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuid1 = new ShortGuid(guid);
        var shortGuid2 = new ShortGuid(guid);

        // Act & Assert
        Assert.That(shortGuid1.Equals(shortGuid2), Is.True);
        Assert.That(shortGuid1 == shortGuid2, Is.True);
    }

    [Test]
    public void Equals_WithDifferentGuid_ReturnsFalse()
    {
        // Arrange
        var shortGuid1 = new ShortGuid(Guid.NewGuid());
        var shortGuid2 = new ShortGuid(Guid.NewGuid());

        // Act & Assert
        Assert.That(shortGuid1.Equals(shortGuid2), Is.False);
        Assert.That(shortGuid1 != shortGuid2, Is.True);
    }

    [Test]
    public void GetHashCode_WithSameGuid_ReturnsSameHashCode()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuid1 = new ShortGuid(guid);
        var shortGuid2 = new ShortGuid(guid);

        // Act
        var hash1 = shortGuid1.GetHashCode();
        var hash2 = shortGuid2.GetHashCode();

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void CompareTo_WithSameGuid_ReturnsZero()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuid1 = new ShortGuid(guid);
        var shortGuid2 = new ShortGuid(guid);

        // Act
        var result = shortGuid1.CompareTo(shortGuid2);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ImplicitConversion_FromGuid_CreatesShortGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        ShortGuid shortGuid = guid;

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(guid));
    }

    [Test]
    public void ImplicitConversion_FromString_CreatesShortGuid()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var encodedString = ShortGuid.Encode(guid);

        // Act
        ShortGuid shortGuid = encodedString;

        // Assert
        Assert.That(shortGuid.ToGuid(), Is.EqualTo(guid));
    }

    [Test]
    public void ImplicitConversion_ToGuid_ReturnsGuid()
    {
        // Arrange
        var originalGuid = Guid.NewGuid();
        var shortGuid = new ShortGuid(originalGuid);

        // Act
        Guid convertedGuid = shortGuid;

        // Assert
        Assert.That(convertedGuid, Is.EqualTo(originalGuid));
    }

    [Test]
    public void ImplicitConversion_ToString_ReturnsEncodedString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var shortGuid = new ShortGuid(guid);
        var expectedString = ShortGuid.Encode(guid);

        // Act
        string convertedString = shortGuid;

        // Assert
        Assert.That(convertedString, Is.EqualTo(expectedString));
    }

    [Test]
    public void Empty_ReturnsEmptyShortGuid()
    {
        // Act
        var emptyShortGuid = ShortGuid.Empty;

        // Assert
        Assert.That(emptyShortGuid.ToGuid(), Is.EqualTo(Guid.Empty));
    }
} 