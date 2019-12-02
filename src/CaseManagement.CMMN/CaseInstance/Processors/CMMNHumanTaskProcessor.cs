using System;
using System.Threading.Tasks;
using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Process;
using CaseManagement.Workflow.Persistence;

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
            if (cmmnPlanItem.FormInstance != null && cmmnPlanItem.FormInstance.Status == ProcessFlowInstanceElementFormStatus.Complete)
            {
                cmmnPlanItem.Complete();
                return true;
            }

            var form = await _formQueryRepository.FindFormById(humanTask.FormId);
            cmmnPlanItem.SetFormInstance(ProcessFlowInstanceElementForm.New(form.Id));
            if (humanTask.IsBlocking)
            {
                return false;
            }

            cmmnPlanItem.Complete();
            return true;
        }
    }
}
