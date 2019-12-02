using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class NotSupportedTaskException : Exception
    {
        public NotSupportedTaskException(string message) : base(message) { }
    }
}
