using System;

namespace eQuantic.Core.Extensions;

/// <summary>
/// Provides extension methods for Exception handling and message extraction.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Gets the deepest inner exception message from the exception chain.
    /// </summary>
    /// <param name="exception">The exception to extract the deep message from.</param>
    /// <returns>The message from the innermost exception.</returns>
    public static string DeepMessage(this Exception exception)
    {
        return GetInnerExceptionMessage(exception);
    }

    /// <summary>
    /// Gets all exception messages from the exception chain, separated by the specified split string.
    /// </summary>
    /// <param name="exception">The exception to extract all messages from.</param>
    /// <param name="split">The string to use as separator between messages.</param>
    /// <returns>A concatenated string of all exception messages in the chain.</returns>
    public static string AllMessages(this Exception exception, string split)
    {
        return GetAllExceptionMessage(exception, split);
    }

    private static string GetAllExceptionMessage(Exception exception, string split)
    {
        var message = exception.Message;
        if (exception.InnerException != null)
            message += split + GetAllExceptionMessage(exception.InnerException, split);

        return message;
    }

    private static string GetInnerExceptionMessage(Exception exception)
    {
        return exception.InnerException != null ? GetInnerExceptionMessage(exception.InnerException) : exception.Message;
    }
}