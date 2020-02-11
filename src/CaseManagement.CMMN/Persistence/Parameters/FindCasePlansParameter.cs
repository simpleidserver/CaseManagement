namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindCasePlansParameter : BaseFindParameter
    {
        public string CaseFileId { get; set; }
        public string Text { get; set; }
        public string CaseOwner { get; set; }
        public string CasePlanId { get; set; }
    }
}
