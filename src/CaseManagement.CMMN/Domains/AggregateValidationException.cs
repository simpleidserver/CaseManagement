using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Infrastructures
{
    public class AggregateValidationException : Exception
    {
        public AggregateValidationException() { }

        public AggregateValidationException(string code, string message)
        {
            Errors = new Dictionary<string, string>
            {
                { code, message}
            };
        }

        public AggregateValidationException(ICollection<KeyValuePair<string, string>> messages)
        {
            Errors = messages;
        }


        public ICollection<KeyValuePair<string, string>> Errors;
    }
}
