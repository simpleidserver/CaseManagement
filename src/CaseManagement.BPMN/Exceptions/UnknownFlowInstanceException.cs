using System;

namespace CaseManagement.BPMN.Exceptions
{
    public class UnknownFlowInstanceException : Exception
    {
        public UnknownFlowInstanceException(string message) : base(message) { }
    }
}
