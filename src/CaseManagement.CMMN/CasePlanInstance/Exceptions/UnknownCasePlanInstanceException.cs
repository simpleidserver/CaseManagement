using System;

namespace CaseManagement.CMMN.CasePlanInstance.Exceptions
{
    public class UnknownCasePlanInstanceException : Exception
    {
        public UnknownCasePlanInstanceException(string caseInstanceId)
        {
            CaseInstanceId = caseInstanceId;
        }

        public string CaseInstanceId { get; set; }
    }
}
