using NUnit.Framework;
using eQuantic.Core.Security.Encryption;
using System.Text.RegularExpressions;

namespace eQuantic.Core.Tests.Security;

[TestFixture]
public class EncryptorTests
{
    private IEncrypting _encryptor;

    [SetUp]
    public void SetUp()
    {
        _encryptor = new Encryptor();
    }

    [Test]
    public void Encode_WithValidPassword_ReturnsHashedString()
    {
        // Arrange
        const string password = "testpassword";

        // Act
        var result = _encryptor.Encode(password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Length, Is.EqualTo(32)); // MD5 hash length
        Assert.That(result, Does.Match("^[A-F0-9]+$")); // Should be uppercase hex
    }

    [Test]
    public void Encode_WithSamePassword_ReturnsSameHash()
    {
        // Arrange
        const string password = "testpassword";

        // Act
        var result1 = _encryptor.Encode(password);
        var result2 = _encryptor.Encode(password);

        // Assert
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void Encode_WithDifferentPasswords_ReturnsDifferentHashes()
    {
        // Arrange
        const string password1 = "testpassword1";
        const string password2 = "testpassword2";

        // Act
        var result1 = _encryptor.Encode(password1);
        var result2 = _encryptor.Encode(password2);

        // Assert
        Assert.That(result1, Is.Not.EqualTo(result2));
    }

    [Test]
    public void Encode_WithEmptyString_ReturnsHash()
    {
        // Arrange
        const string password = "";

        // Act
        var result = _encryptor.Encode(password);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Length, Is.EqualTo(32));
    }

    [Test]
    public void Random_WithValidLength_ReturnsStringOfCorrectLength()
    {
        // Arrange
        const int length = 10;

        // Act
        var result = _encryptor.Random(length);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(length));
    }

    [Test]
    public void Random_WithZeroLength_ReturnsEmptyString()
    {
        // Arrange
        const int length = 0;

        // Act
        var result = _encryptor.Random(length);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void Random_ContainsOnlyValidCharacters()
    {
        // Arrange
        const int length = 50;
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        // Act
        var result = _encryptor.Random(length);

        // Assert
        Assert.That(result, Is.Not.Null);
        foreach (char c in result)
        {
            Assert.That(validChars.Contains(c), Is.True);
        }
    }

    [Test]
    public void Random_WithSameLength_ReturnsDifferentStrings()
    {
        // Arrange
        const int length = 20;

        // Act
        var result1 = _encryptor.Random(length);
        var result2 = _encryptor.Random(length);

        // Assert
        Assert.That(result1, Is.Not.EqualTo(result2));
    }

    [Test]
    public void Random_WithLargeLength_WorksCorrectly()
    {
        // Arrange
        const int length = 1000;

        // Act
        var result = _encryptor.Random(length);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(length));
    }

    [Test]
    public void Random_OnlyAlphanumericCharacters()
    {
        // Arrange
        const int length = 100;

        // Act
        var result = _encryptor.Random(length);

        // Assert
        Assert.That(result, Does.Match("^[a-zA-Z0-9]+$"));
    }
} 