using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Process;
using CaseManagement.Workflow.Persistence;
using System;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNHumanTaskProcessor : BaseCMMNTaskProcessor
    {
        private readonly IFormQueryRepository _formQueryRepository;
        
        public CMMNHumanTaskProcessor(IFormQueryRepository formQueryRepository)
        {
            _formQueryRepository = formQueryRepository;
        }
        
        public override Type PlanItemDefinitionType => typeof(CMMNHumanTask);

        public override async Task<bool> Run(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            var humanTask = cmmnPlanItem.PlanItemDefinition as CMMNHumanTask;
            var formInstance = pf.GetFormInstance(cmmnPlanItem.Id);
            if (formInstance != null && formInstance.Status == ProcessFlowInstanceElementFormStatus.Complete)
            {
                cmmnPlanItem.Complete();
                return true;
            }

            if (humanTask.IsBlocking)
            {
                return false;
            }

            cmmnPlanItem.Complete();
            return true;
        }
    }
}
