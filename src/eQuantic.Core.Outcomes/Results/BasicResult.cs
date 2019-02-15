using System;
using System.Collections.Generic;
using System.Linq;

namespace eQuantic.Core.Outcomes.Results
{
    public class BasicResult : IBasicResult
    {
        private bool _success;
        private string _message;
        private List<string> _messages;

        public bool Success
        {
            get { return _success; }
            set
            {
                if(value) Status = ResultStatus.Success;
                _success = value;
            }
        }

        public int? ErrorCode { get; set; }

        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message) && Messages.Any())
                    return Messages.FirstOrDefault();

                return _message;
            }
            set { _message = value; }
        }

        public List<string> Messages
        {
            get { return _messages; }
            set
            {
                Success = false;
                Status = ResultStatus.Error;
                _messages = value;
            }
        }

        public ResultStatus Status { get; set; }

        public BasicResult()
        {
            Success = false;
            _messages = new List<string>();
        }
    }
}