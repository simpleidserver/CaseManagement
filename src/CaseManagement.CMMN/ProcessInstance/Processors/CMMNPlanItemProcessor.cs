using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.ProcessInstance.Processors
{
    public class CMMNPlanItemProcessor : BaseProcessFlowElementProcessor
    {
        public override Type ProcessFlowElementType => typeof(CMMNPlanItem);

        protected override Task<bool> HandleProcessFlowInstance(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var planItem = (CMMNPlanItem)pfe;
            var processTask = (CMMNProcessTask)planItem.PlanItemDefinition;
            // TODO : check the plan definition status and execute the correct logic.
            if (processTask.State == CMMNTaskStates.Active)
            {
                planItem.Complete();
                return Task.FromResult(true);
            }

            if (!HandlePlanItemControl(planItem, context))
            {
                return Task.FromResult(false);
            }

            // TODO : execute the plan item definition.
            planItem.Complete();
            return Task.FromResult(true);
        }

        protected bool HandlePlanItemControl(CMMNPlanItem pfe, ProcessFlowInstanceExecutionContext context)
        {
            var planItem = (CMMNPlanItem)pfe;
            if (planItem.SEntries.Any() && !planItem.SEntries.Any(s => CheckSEntry(s, context)))
            {
                return false;
            }

            if (planItem.PlanItemControl != null)
            {
                if (planItem.PlanItemControl is CMMNManualActivationRule)
                {
                    planItem.Enable();
                    return false;
                }
            }

            planItem.Start();
            return true;
        }

        private bool CheckSEntry(CMMNSEntry sEntry, ProcessFlowInstanceExecutionContext context)
        {
            foreach (var onPart in sEntry.OnParts)
            {
                if (onPart is CMMNPlanItemOnPart)
                {
                    if (!string.IsNullOrWhiteSpace(onPart.SourceRef))
                    {
                        var elt = context.GetPlanItem(onPart.SourceRef);
                        if (elt == null || elt.Events.Last().Transition != onPart.StandardEvent)
                        {
                            return false;
                        }
                    }
                }
            }

            if (sEntry.IfPart != null)
            {
                return ExpressionParser.IsValid(sEntry.IfPart.Condition, context);
            }

            return true;
        }
    }
}
