using System;

namespace eQuantic.Core.Extensions;

public static class ExceptionExtensions
{
    public static string DeepMessage(this Exception exception)
    {
        return GetInnerExceptionMessage(exception);
    }

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