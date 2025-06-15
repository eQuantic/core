namespace eQuantic.Core.MediaFormatter;

/// <summary>
/// Represents an HTTP file with its metadata and content.
/// </summary>
public class HttpFile
{
    /// <summary>
    /// Gets or sets the file name.
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// Gets or sets the media type (MIME type) of the file.
    /// </summary>
    public string MediaType { get; set; }
    
    /// <summary>
    /// Gets or sets the file content as a byte array.
    /// </summary>
    public byte[] Buffer { get; set; }

    /// <summary>
    /// Initializes a new instance of the HttpFile class.
    /// </summary>
    public HttpFile() { }

    /// <summary>
    /// Initializes a new instance of the HttpFile class with the specified parameters.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="mediaType">The media type (MIME type) of the file.</param>
    /// <param name="buffer">The file content as a byte array.</param>
    public HttpFile(string fileName, string mediaType, byte[] buffer)
    {
        FileName = fileName;
        MediaType = mediaType;
        Buffer = buffer;
    }
}