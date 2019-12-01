using System;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNTaskProcessor : BaseCMMNTaskProcessor
    {
        public override Type PlanItemDefinitionType => typeof(CMMNTask);

        public override Task<bool> Run(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            return Task.FromResult(true);
        }
    }
}
