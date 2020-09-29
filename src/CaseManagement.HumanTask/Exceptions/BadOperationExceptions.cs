using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Exceptions
{
    public class BadOperationExceptions : Exception
    {
        public BadOperationExceptions(string validationError) : this(new List<string> { validationError })
        {

        }

        public BadOperationExceptions(ICollection<string> validationErrors)
        {
            ValidationErrors = validationErrors;
        }

        public ICollection<string> ValidationErrors { get; set; }
    }
}
