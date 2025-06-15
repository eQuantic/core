using NUnit.Framework;
using eQuantic.Core.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eQuantic.Core.Tests.Collections;

[TestFixture]
public class PagedListTests
{
    [Test]
    public void Constructor_WithValidCollectionAndTotal_CreatesPagedList()
    {
        // Arrange
        var items = new List<string> { "item1", "item2", "item3" };
        const long total = 10;

        // Act
        var pagedList = new PagedList<string>(items, total);

        // Assert
        Assert.That(pagedList.Count, Is.EqualTo(3));
        Assert.That(pagedList.TotalCount, Is.EqualTo(total));
        Assert.That(pagedList, Is.EquivalentTo(items));
    }

    [Test]
    public void Constructor_WithEmptyCollection_CreatesEmptyPagedList()
    {
        // Arrange
        var items = new List<string>();
        const long total = 0;

        // Act
        var pagedList = new PagedList<string>(items, total);

        // Assert
        Assert.That(pagedList.Count, Is.EqualTo(0));
        Assert.That(pagedList.TotalCount, Is.EqualTo(total));
        Assert.That(pagedList, Is.Empty);
    }

    [Test]
    public void Constructor_WithNullCollection_ThrowsException()
    {
        // Arrange
        List<string>? items = null;
        const long total = 10;

        // Act & Assert
        Assert.Throws<System.ArgumentNullException>(() => new PagedList<string>(items!, total));
    }

    [Test]
    public void Total_Property_ReturnsCorrectValue()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3, 4, 5 };
        const long expectedTotal = 100;

        // Act
        var pagedList = new PagedList<int>(items, expectedTotal);

        // Assert
        Assert.That(pagedList.TotalCount, Is.EqualTo(expectedTotal));
    }

    [Test]
    public void PagedList_ImplementsIPagedEnumerable()
    {
        // Arrange
        var items = new List<string> { "test1", "test2" };
        const long total = 50;

        // Act
        var pagedList = new PagedList<string>(items, total);

        // Assert
        Assert.That(pagedList, Is.InstanceOf<IPagedEnumerable<string>>());
    }

    [Test]
    public void PagedList_InheritsFromList()
    {
        // Arrange
        var items = new List<string> { "test1", "test2" };
        const long total = 50;

        // Act
        var pagedList = new PagedList<string>(items, total);

        // Assert
        Assert.That(pagedList, Is.InstanceOf<List<string>>());
    }

    [Test]
    public void PagedList_CanAddItems()
    {
        // Arrange
        var items = new List<string> { "item1", "item2" };
        var pagedList = new PagedList<string>(items, 10);

        // Act
        pagedList.Add("item3");

        // Assert
        Assert.That(pagedList.Count, Is.EqualTo(3));
        Assert.That(pagedList.Contains("item3"), Is.True);
    }

    [Test]
    public void PagedList_CanRemoveItems()
    {
        // Arrange
        var items = new List<string> { "item1", "item2", "item3" };
        var pagedList = new PagedList<string>(items, 10);

        // Act
        var removed = pagedList.Remove("item2");

        // Assert
        Assert.That(removed, Is.True);
        Assert.That(pagedList.Count, Is.EqualTo(2));
        Assert.That(pagedList.Contains("item2"), Is.False);
    }

    [Test]
    public void PagedList_CanBeEnumerated()
    {
        // Arrange
        var items = new List<string> { "a", "b", "c" };
        var pagedList = new PagedList<string>(items, 10);

        // Act
        var enumeratedItems = pagedList.ToList();

        // Assert
        Assert.That(enumeratedItems, Is.EquivalentTo(items));
    }

    [Test]
    public void PagedList_WithDifferentTypes_WorksCorrectly()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3, 4, 5 };
        const long total = 25;

        // Act
        var pagedList = new PagedList<int>(items, total);

        // Assert
        Assert.That(pagedList.Count, Is.EqualTo(5));
        Assert.That(pagedList.TotalCount, Is.EqualTo(total));
        Assert.That(pagedList.Sum(), Is.EqualTo(15));
    }

    [Test]
    public void PagedList_WithComplexObjects_WorksCorrectly()
    {
        // Arrange
        var items = new List<TestObject>
        {
            new TestObject { Id = 1, Name = "Test1" },
            new TestObject { Id = 2, Name = "Test2" }
        };
        const long total = 20;

        // Act
        var pagedList = new PagedList<TestObject>(items, total);

        // Assert
        Assert.That(pagedList.Count, Is.EqualTo(2));
        Assert.That(pagedList.TotalCount, Is.EqualTo(total));
        Assert.That(pagedList.First().Name, Is.EqualTo("Test1"));
    }

    private class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
} 