using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Common.Exceptions
{
    public class AggregateValidationException : Exception
    {
        public AggregateValidationException(ICollection<KeyValuePair<string, string>> content)
        {
            Content = content;
        }

        public ICollection<KeyValuePair<string, string>> Content { get; set; }
    }
}
