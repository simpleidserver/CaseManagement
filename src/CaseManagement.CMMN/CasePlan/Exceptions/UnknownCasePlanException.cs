using System;

namespace CaseManagement.CMMN.CasePlan.Exceptions
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
