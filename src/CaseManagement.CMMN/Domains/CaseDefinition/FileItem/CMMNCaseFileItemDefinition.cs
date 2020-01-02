namespace CaseManagement.CMMN.Domains
{
    public class CMMNCaseFileItemDefinition : CMMNWorkflowElementDefinition
    {
        public CMMNCaseFileItemDefinition(string id, string name) : base(id, name)
        {
            Type = CMMNWorkflowElementTypes.CaseFileItem;
        }
        
        public CMMNMultiplicities Multiplicity { get; set; }
        public string Definition { get; set; }
    }
}
