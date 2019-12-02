using System;

namespace CaseManagement.CMMN.CaseInstance.Exceptions
{
    public class UnknownCaseInstanceElementException : Exception
    {
        public UnknownCaseInstanceElementException(string caseInstanceId, string caseInstanceElementId)
        {
            CaseInstanceId = caseInstanceId;
            CaseInstanceElementId = caseInstanceElementId;
        }

        public string CaseInstanceId { get; set; }
        public string CaseInstanceElementId { get; set; }
    }
}
