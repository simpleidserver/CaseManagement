namespace CaseManagement.CMMN.Domains
{
    public class CaseFileItemDefinition : CasePlanElement
    {
        public CaseFileItemDefinition(string id, string name) : base(id, name)
        {
            Type = CaseElementTypes.CaseFileItem;
        }
        
        public Multiplicities Multiplicity { get; set; }
        public string Definition { get; set; }
    }
}
