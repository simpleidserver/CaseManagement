using System;

namespace CaseManagement.CMMN.CasePlanInstance.Exceptions
{
    public class UnknownCasePlanElementInstanceException : Exception
    {
        public UnknownCasePlanElementInstanceException(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
