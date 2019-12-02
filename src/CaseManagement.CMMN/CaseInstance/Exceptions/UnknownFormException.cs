using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class UnknownFormException : Exception
    {
        public UnknownFormException(string formId)
        {
            FormId = formId;
        }

        public string FormId { get; set; }
    }
}
