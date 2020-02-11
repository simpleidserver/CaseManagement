using System;

namespace CaseManagement.CMMN.CasePlanInstance.Exceptions
{
    public class NotSupportedTaskException : Exception
    {
        public NotSupportedTaskException(string message) : base(message) { }
    }
}
