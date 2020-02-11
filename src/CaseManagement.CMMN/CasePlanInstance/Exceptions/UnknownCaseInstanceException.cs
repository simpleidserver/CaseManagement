using System;

namespace CaseManagement.CMMN.CasePlanInstance.Exceptions
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
