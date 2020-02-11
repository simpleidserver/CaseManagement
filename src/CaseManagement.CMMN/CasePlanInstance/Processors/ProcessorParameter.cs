using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class ProcessorParameter
    {
        public ProcessorParameter(CasePlanAggregate caseDefinition, Domains.CasePlanInstanceAggregate caseInstance, CaseElementInstance caseElementInstance)
        {
            CaseDefinition = caseDefinition;
            CaseInstance = caseInstance;
            CaseElementInstance = caseElementInstance;
        }

        public CasePlanAggregate CaseDefinition { get; set; }
        public Domains.CasePlanInstanceAggregate CaseInstance { get; set; }
        public CaseElementInstance CaseElementInstance { get; set; }
    }
}
