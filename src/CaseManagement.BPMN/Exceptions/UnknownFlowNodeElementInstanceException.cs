using System;

namespace CaseManagement.BPMN.Exceptions
{
    public class UnknownFlowNodeElementInstanceException : Exception
    {
        public UnknownFlowNodeElementInstanceException(string msg) : base(msg) { }
    }
}
