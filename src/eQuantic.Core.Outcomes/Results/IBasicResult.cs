using System;
using System.Collections.Generic;
using System.Linq;

namespace eQuantic.Core.Outcomes.Results
{
    public interface IBasicResult
    {
        bool Success { get; }
        ResultStatus Status { get; }
        int? ErrorCode { get; }
        string Message { get; }
        List<string> Messages { get; }
    }
}