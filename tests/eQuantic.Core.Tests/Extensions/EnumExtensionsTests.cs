using NUnit.Framework;
using eQuantic.Core.Extensions;
using eQuantic.Core.Attributes;
using System;
using System.ComponentModel;
using System.Linq;

namespace eQuantic.Core.Tests.Extensions;

[TestFixture]
public class EnumExtensionsTests
{
    [Flags]
    private enum TestFlags
    {
        None = 0,
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4,
        Flag4 = 8,
        Combined = Flag1 | Flag2
    }

    private enum TestEnum
    {
        [System.ComponentModel.Description("First Value")]
        Value1,
        
        [System.ComponentModel.Description("Second Value")]
        Value2,
        
        Value3
    }

    [Test]
    public void GetDescription_WithDescriptionAttribute_ReturnsDescription()
    {
        // Arrange
        var enumValue = TestEnum.Value1;

        // Act
        var result = enumValue.GetDescription();

        // Assert
        Assert.That(result, Is.EqualTo("First Value"));
    }

    [Test]
    public void GetDescription_WithoutDescriptionAttribute_ReturnsEnumName()
    {
        // Arrange
        var enumValue = TestEnum.Value3;

        // Act
        var result = enumValue.GetDescription();

        // Assert
        Assert.That(result, Is.EqualTo("Value3"));
    }

    [Test]
    public void GetList_WithEnumType_ReturnsAllValues()
    {
        // Arrange
        var enumValue = TestEnum.Value1;

        // Act
        var result = enumValue.GetList<TestEnum>();

        // Assert
        Assert.That(result, Contains.Item(TestEnum.Value1));
        Assert.That(result, Contains.Item(TestEnum.Value2));
        Assert.That(result, Contains.Item(TestEnum.Value3));
        Assert.That(result.Count, Is.EqualTo(3));
    }

    [Test]
    public void GetFlags_WithFlagsEnum_ReturnsIndividualFlags()
    {
        // Arrange
        var enumValue = TestFlags.Flag1 | TestFlags.Flag2;

        // Act
        var result = EnumExtension.GetFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.Combined)); // Combined = Flag1 | Flag2
        Assert.That(result.Count(), Is.GreaterThan(0));
    }

    [Test]
    public void GetFlags_WithSingleFlag_ReturnsSingleFlag()
    {
        // Arrange
        var enumValue = TestFlags.Flag1;

        // Act
        var result = EnumExtension.GetFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.Flag1));
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public void GetFlags_WithNoneFlag_ReturnsNone()
    {
        // Arrange
        var enumValue = TestFlags.None;

        // Act
        var result = EnumExtension.GetFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.None));
    }

    [Test]
    public void GetIndividualFlags_WithCombinedFlags_ReturnsIndividualFlags()
    {
        // Arrange
        var enumValue = TestFlags.Combined; // Flag1 | Flag2

        // Act
        var result = EnumExtension.GetIndividualFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.Flag1));
        Assert.That(result, Contains.Item(TestFlags.Flag2));
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetIndividualFlags_WithMultipleFlags_ReturnsAllIndividualFlags()
    {
        // Arrange
        var enumValue = TestFlags.Flag1 | TestFlags.Flag3 | TestFlags.Flag4;

        // Act
        var result = EnumExtension.GetIndividualFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.Flag1));
        Assert.That(result, Contains.Item(TestFlags.Flag3));
        Assert.That(result, Contains.Item(TestFlags.Flag4));
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetIndividualFlags_WithSingleFlag_ReturnsSingleFlag()
    {
        // Arrange
        var enumValue = TestFlags.Flag2;

        // Act
        var result = EnumExtension.GetIndividualFlags(enumValue);

        // Assert
        Assert.That(result, Contains.Item(TestFlags.Flag2));
        Assert.That(result.Count(), Is.EqualTo(1));
    }

    [Test]
    public void HasFlag_WithValidFlag_ReturnsTrue()
    {
        // Arrange
        var enumValue = TestFlags.Flag1 | TestFlags.Flag2;

        // Act
        var result = enumValue.HasFlag(TestFlags.Flag1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void HasFlag_WithInvalidFlag_ReturnsFalse()
    {
        // Arrange
        var enumValue = TestFlags.Flag1 | TestFlags.Flag2;

        // Act
        var result = enumValue.HasFlag(TestFlags.Flag3);

        // Assert
        Assert.That(result, Is.False);
    }

    // Mock resource class for testing
    private static class TestResources
    {
        public static string FirstValue => "Localized First Value";
    }
} 