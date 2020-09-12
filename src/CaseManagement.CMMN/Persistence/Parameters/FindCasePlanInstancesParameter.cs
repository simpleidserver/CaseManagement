using CaseManagement.CMMN.Common.Parameters;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCasePlanInstancesParameter : BaseSearchParameter
    {
        public string CasePlanId { get; set; }
    }
}
