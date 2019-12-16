using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class CaseInvalidOperationException : Exception
    {
        public CaseInvalidOperationException(string message) : base(message) { }
    }
}