using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNPlanItemProcessor : BaseProcessFlowElementProcessor
    {
        private readonly IEnumerable<ICMMNPlanItemDefinitionProcessor> _planItemDefinitionProcessors;
        
        public CMMNPlanItemProcessor(IEnumerable<ICMMNPlanItemDefinitionProcessor> planItemDefinitionProcessors)
        {
            _planItemDefinitionProcessors = planItemDefinitionProcessors;
        }

        public override Type ProcessFlowElementType => typeof(CMMNPlanItem);

        protected override Task<bool> HandleProcessFlowInstance(ProcessFlowInstance pf, ProcessFlowInstanceElement pfe)
        {
            var planItem = (CMMNPlanItem)pfe;
            var planItemDef = _planItemDefinitionProcessors.FirstOrDefault(p => p.PlanItemDefinitionType == planItem.PlanItemDefinition.GetType());
            if (planItemDef == null)
            {
                return Task.FromResult(false);
            }

            return planItemDef.Handle(planItem, pf);
        }
    }
}
