using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public interface ICMMNPlanItemDefinitionProcessor
    {
        Type PlanItemDefinitionType { get; }
        Task<bool> Handle(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf);
    }
}
