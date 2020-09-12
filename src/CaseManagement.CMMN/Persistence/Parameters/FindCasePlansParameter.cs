using CaseManagement.CMMN.Common.Parameters;

namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCasePlansParameter : BaseSearchParameter
    {
        public string CaseFileId { get; set; }
        public string Text { get; set; }
        public string CaseOwner { get; set; }
        public string CasePlanId { get; set; }
        public bool TakeLatest { get; set; }
    }
}
