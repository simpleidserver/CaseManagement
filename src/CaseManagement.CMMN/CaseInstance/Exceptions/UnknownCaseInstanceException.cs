using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class UnknownCaseInstanceException : Exception
    {
        public UnknownCaseInstanceException(string caseInstanceId)
        {
            CaseInstanceId = caseInstanceId;
        }

        public string CaseInstanceId { get; set; }
    }
}
