using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Common.Exceptions
{
    public class AggregateValidationException : Exception
    {
        public AggregateValidationException(ICollection<KeyValuePair<string, string>> errors)
        {
            Errors = errors;
        }

        public ICollection<KeyValuePair<string, string>> Errors { get; set; }
    }
}
