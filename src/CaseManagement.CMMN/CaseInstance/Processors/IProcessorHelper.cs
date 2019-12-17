using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public interface IProcessorHelper
    {
        RepetitionRuleResultTypes? HandleRepetitionRule(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf);
    }
}
