namespace CaseManagement.CMMN.Persistence.Parameters
{
    public class FindWorkflowDefinitionsParameter : BaseFindParameter
    {
        public string CaseFileId { get; set; }
        public string Text { get; set; }
        public string CaseOwner { get; set; }
    }
}
