using NUnit.Framework;
using eQuantic.Core.MediaFormatter;
using System.Text;

namespace eQuantic.Core.Tests.MediaFormatter;

[TestFixture]
public class HttpFileTests
{
    [Test]
    public void Constructor_Default_CreatesEmptyHttpFile()
    {
        // Act
        var httpFile = new HttpFile();

        // Assert
        Assert.That(httpFile.FileName, Is.Null);
        Assert.That(httpFile.MediaType, Is.Null);
        Assert.That(httpFile.Buffer, Is.Null);
    }

    [Test]
    public void Constructor_WithParameters_SetsProperties()
    {
        // Arrange
        const string fileName = "test.txt";
        const string mediaType = "text/plain";
        var buffer = Encoding.UTF8.GetBytes("Hello World");

        // Act
        var httpFile = new HttpFile(fileName, mediaType, buffer);

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
    }

    [Test]
    public void Properties_CanBeSetIndividually()
    {
        // Arrange
        var httpFile = new HttpFile();
        const string fileName = "document.pdf";
        const string mediaType = "application/pdf";
        var buffer = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        httpFile.FileName = fileName;
        httpFile.MediaType = mediaType;
        httpFile.Buffer = buffer;

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
    }

    [Test]
    public void Constructor_WithNullValues_AcceptsNullValues()
    {
        // Act
        var httpFile = new HttpFile(null, null, null);

        // Assert
        Assert.That(httpFile.FileName, Is.Null);
        Assert.That(httpFile.MediaType, Is.Null);
        Assert.That(httpFile.Buffer, Is.Null);
    }

    [Test]
    public void Constructor_WithEmptyValues_AcceptsEmptyValues()
    {
        // Arrange
        const string fileName = "";
        const string mediaType = "";
        var buffer = new byte[0];

        // Act
        var httpFile = new HttpFile(fileName, mediaType, buffer);

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
        Assert.That(httpFile.Buffer.Length, Is.EqualTo(0));
    }

    [Test]
    public void HttpFile_WithImageFile_WorksCorrectly()
    {
        // Arrange
        const string fileName = "image.jpg";
        const string mediaType = "image/jpeg";
        var buffer = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }; // JPEG header

        // Act
        var httpFile = new HttpFile(fileName, mediaType, buffer);

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
        Assert.That(httpFile.Buffer.Length, Is.EqualTo(4));
    }

    [Test]
    public void HttpFile_WithLargeFile_WorksCorrectly()
    {
        // Arrange
        const string fileName = "largefile.bin";
        const string mediaType = "application/octet-stream";
        var buffer = new byte[1024 * 1024]; // 1MB
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(i % 256);
        }

        // Act
        var httpFile = new HttpFile(fileName, mediaType, buffer);

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
        Assert.That(httpFile.Buffer.Length, Is.EqualTo(1024 * 1024));
    }

    [Test]
    public void HttpFile_WithSpecialCharactersInFileName_WorksCorrectly()
    {
        // Arrange
        const string fileName = "файл с пробелами и спец символами.txt";
        const string mediaType = "text/plain; charset=utf-8";
        var buffer = Encoding.UTF8.GetBytes("Content with special characters: áéíóú");

        // Act
        var httpFile = new HttpFile(fileName, mediaType, buffer);

        // Assert
        Assert.That(httpFile.FileName, Is.EqualTo(fileName));
        Assert.That(httpFile.MediaType, Is.EqualTo(mediaType));
        Assert.That(httpFile.Buffer, Is.EqualTo(buffer));
    }
} 