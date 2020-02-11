using System;

namespace CaseManagement.CMMN.CasePlanInstance.Exceptions
{
    public class CaseInvalidOperationException : Exception
    {
        public CaseInvalidOperationException(string message) : base(message) { }
    }
}