namespace CaseManagement.CMMN.Domains
{
    public class CaseManagementProcessAggregate : ProcessAggregate
    {
        public CaseManagementProcessAggregate() : base(CMMNConstants.ProcessImplementationTypes.CASEMANAGEMENTCALLBACK)
        {
        }
        
        public string AssemblyQualifiedName { get; set; }
    }
}
