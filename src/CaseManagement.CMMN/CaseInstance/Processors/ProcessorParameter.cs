using CaseManagement.CMMN.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class ProcessorParameter
    {
        public ProcessorParameter(CaseDefinition caseDefinition, Domains.CaseInstance caseInstance, CaseElementInstance caseElementInstance)
        {
            CaseDefinition = caseDefinition;
            CaseInstance = caseInstance;
            CaseElementInstance = caseElementInstance;
        }

        public CaseDefinition CaseDefinition { get; set; }
        public Domains.CaseInstance CaseInstance { get; set; }
        public CaseElementInstance CaseElementInstance { get; set; }
    }
}
