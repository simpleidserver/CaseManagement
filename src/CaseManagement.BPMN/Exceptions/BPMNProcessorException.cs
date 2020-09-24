using System;

namespace CaseManagement.BPMN.Exceptions
{
    public class BPMNProcessorException : Exception
    {
        public BPMNProcessorException(string message) : base(message) { }
    }
}
