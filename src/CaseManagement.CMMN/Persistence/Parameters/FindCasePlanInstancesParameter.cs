using CaseManagement.Common.Parameters;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCasePlanInstancesParameter : BaseSearchParameter
    {
        public string CasePlanId { get; set; }
        public string CaseFileId { get; set; }
    }
}
