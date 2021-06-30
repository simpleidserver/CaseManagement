using System;

namespace CaseManagement.BPMN.Exceptions
{
    public class UnknownDelegateConfigurationException : Exception
    {
        public UnknownDelegateConfigurationException(string message) : base(message) { }
    }
}
