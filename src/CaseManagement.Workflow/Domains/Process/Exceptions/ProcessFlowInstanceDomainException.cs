using System;
using System.Collections.Generic;

namespace CaseManagement.Workflow.Domains.Process.Exceptions
{
    public class ProcessFlowInstanceDomainException : Exception
    {
        public ProcessFlowInstanceDomainException() { }

        public ProcessFlowInstanceDomainException(string code, string message)
        {
            Errors = new Dictionary<string, string>
            {
                { code, message}
            };
        }

        public ProcessFlowInstanceDomainException(ICollection<KeyValuePair<string, string>> messages)
        {
            Errors = messages;
        }

        public ICollection<KeyValuePair<string, string>> Errors;
    }
}
