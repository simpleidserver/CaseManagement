using System;

namespace CaseManagement.CMMN.Common.Exceptions
{
    public class UnknownCasePlanException : Exception
    {
        public UnknownCasePlanException(string casePlanId)
        {
            CasePlanId = casePlanId;
        }

        public string CasePlanId { get; set; }
    }
}
