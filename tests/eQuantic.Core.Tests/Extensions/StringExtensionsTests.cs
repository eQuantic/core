using NUnit.Framework;
using eQuantic.Core.Extensions;

namespace eQuantic.Core.Tests.Extensions;

[TestFixture]
public class StringExtensionsTests
{
    [Test]
    public void LeftOf_WithValidCharacter_ReturnsSubstring()
    {
        // Arrange
        var input = "Hello.World.Test";

        // Act
        var result = input.LeftOf('.');

        // Assert
        Assert.That(result, Is.EqualTo("Hello"));
    }

    [Test]
    public void LeftOf_WithCharacterNotFound_ReturnsOriginalString()
    {
        // Arrange
        var input = "HelloWorld";

        // Act
        var result = input.LeftOf('.');

        // Assert
        Assert.That(result, Is.EqualTo("HelloWorld"));
    }

    [Test]
    public void ToTitleCase_WithLowercaseString_ReturnsCapitalized()
    {
        // Arrange
        var input = "hello world";

        // Act
        var result = input.ToTitleCase();

        // Assert
        Assert.That(result, Is.EqualTo("Hello World"));
    }

    [Test]
    public void ToTitleCase_WithMixedCaseString_ReturnsCapitalized()
    {
        // Arrange
        var input = "hELLo WoRLd";

        // Act
        var result = input.ToTitleCase();

        // Assert
        Assert.That(result, Is.EqualTo("HELLo WoRLd")); // ToTitleCase preserves existing case
    }

    [Test]
    public void SplitOnCapitals_WithCamelCase_ReturnsSeparatedWords()
    {
        // Arrange
        var input = "HelloWorld";

        // Act
        var result = input.SplitOnCapitals();

        // Assert
        Assert.That(result, Is.EqualTo(" Hello World")); // Method adds separator before each capital
    }

    [Test]
    public void SplitOnCapitals_WithCustomSeparator_UsesCustomSeparator()
    {
        // Arrange
        var input = "HelloWorld";

        // Act
        var result = input.SplitOnCapitals('-');

        // Assert
        Assert.That(result, Is.EqualTo("-Hello-World")); // Method adds separator before each capital
    }

    [Test]
    public void Slugify_WithSpecialCharacters_ReturnsSlug()
    {
        // Arrange
        var input = "Hello World! This is a test.";

        // Act
        var result = input.Slugify();

        // Assert
        Assert.That(result, Is.EqualTo("hello-world-this-is-a-test"));
    }

    [Test]
    public void Slugify_WithAccentedCharacters_RemovesAccents()
    {
        // Arrange
        var input = "Café & Résumé";

        // Act
        var result = input.Slugify();

        // Assert
        Assert.That(result, Is.EqualTo("cafe-resume"));
    }

    [Test]
    public void RemoveAccents_WithAccentedCharacters_RemovesAccents()
    {
        // Arrange
        var input = "Café Résumé";

        // Act
        var result = input.RemoveAccents();

        // Assert
        Assert.That(result, Is.EqualTo("Cafe Resume"));
    }

    [Test]
    public void SingleSpacedTrim_WithMultipleSpaces_ReturnsNormalizedString()
    {
        // Arrange
        var input = "  Hello    World  ";

        // Act
        var result = input.SingleSpacedTrim();

        // Assert
        Assert.That(result, Is.EqualTo("Hello World"));
    }

    [Test]
    public void RightOf_WithValidCharacter_ReturnsSubstring()
    {
        // Arrange
        var input = "Hello.World.Test";

        // Act
        var result = input.RightOf('.', 1);

        // Assert
        Assert.That(result, Is.EqualTo("World.Test"));
    }

    [Test]
    public void RightOf_WithSecondOccurrence_ReturnsCorrectSubstring()
    {
        // Arrange
        var input = "Hello.World.Test";

        // Act
        var result = input.RightOf('.', 2);

        // Assert
        Assert.That(result, Is.EqualTo("Test"));
    }

    [Test]
    public void Between_WithValidCharacters_ReturnsSubstring()
    {
        // Arrange
        var input = "Hello(World)Test";

        // Act
        var result = input.Between('(', ')');

        // Assert
        Assert.That(result, Is.EqualTo("World"));
    }

    [Test]
    public void Count_WithValidCharacter_ReturnsCorrectCount()
    {
        // Arrange
        var input = "Hello.World.Test";

        // Act
        var result = input.Count('.');

        // Assert
        Assert.That(result, Is.EqualTo(2));
    }

    [Test]
    public void Multiply_WithValidMultiplier_ReturnsRepeatedString()
    {
        // Arrange
        var input = "Hello";

        // Act
        var result = input.Multiply(3);

        // Assert
        Assert.That(result, Is.EqualTo("HelloHelloHello"));
    }

    [Test]
    public void Multiply_WithZeroMultiplier_ReturnsEmptyString()
    {
        // Arrange
        var input = "Hello";

        // Act
        var result = input.Multiply(0);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Rightmost_WithValidString_ReturnsLastCharacter()
    {
        // Arrange
        var input = "Hello";

        // Act
        var result = input.Rightmost();

        // Assert
        Assert.That(result, Is.EqualTo('o'));
    }

    [Test]
    public void RemoveSpecialCharacters_WithSpecialChars_ReturnsCleanString()
    {
        // Arrange
        var input = "Hello@World#Test!";

        // Act
        var result = input.RemoveSpecialCharacters();

        // Assert
        Assert.That(result, Is.EqualTo("Hello_World_Test_"));
    }

    [Test]
    public void RemoveSpecialCharacters_WithCustomReplacement_UsesCustomReplacement()
    {
        // Arrange
        var input = "Hello@World#Test!";

        // Act
        var result = input.RemoveSpecialCharacters('-');

        // Assert
        Assert.That(result, Is.EqualTo("Hello-World-Test-"));
    }
} 