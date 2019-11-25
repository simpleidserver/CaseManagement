using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class CMMNPlanItemProcessor : BaseProcessFlowElementProcessor
    {
        public override Type ProcessFlowElementType => typeof(CMMNPlanItem);

        protected override Task<bool> HandleProcessFlowInstance(ProcessFlowInstanceElement pfe, ProcessFlowInstanceExecutionContext context)
        {
            var planItem = (CMMNPlanItem)pfe;
            var processTask = planItem.PlanItemDefinition as CMMNProcessTask;
            if (processTask != null)
            {
                return HandleProcessTask(planItem, processTask, context);
            }

            return Task.FromResult(true);
        }

        protected Task<bool> HandleProcessTask(CMMNPlanItem planItem, CMMNProcessTask processTask, ProcessFlowInstanceExecutionContext context)
        {
            if (planItem.ExitCriterions.Any() && planItem.ExitCriterions.Any(s => CheckCriterion(s, context)))
            {
                planItem.Terminate();
                return Task.FromResult(true);
            }
            
            if (processTask.State == CMMNTaskStates.Available)
            {
                if (planItem.EntryCriterions.Any() && !planItem.EntryCriterions.Any(s => CheckCriterion(s, context)))
                {
                    return Task.FromResult(true);
                }

                if (planItem.PlanItemControl != null)
                {
                    var manualActivationRule = planItem.PlanItemControl as CMMNManualActivationRule;
                    if (manualActivationRule != null)
                    {
                        // Note : at the moment the ContextRef is ignored.
                        if (ExpressionParser.IsValid(manualActivationRule.Expression.Body, context))
                        {
                            planItem.Enable();
                            return Task.FromResult(false);
                        }
                    }
                }

                planItem.Start();
            }

            if (processTask.State == CMMNTaskStates.Active)
            {
                // TODO : Execute the plan item definition.
                planItem.Complete();
                return Task.FromResult(true);
            }

            return Task.FromResult(true);
        }

        protected Task<bool> HandleHumanTask()
        {
            // ADD A USER TASK.
            return Task.FromResult(true);
        }

        private bool CheckCriterion(CMMNCriterion sCriterion, ProcessFlowInstanceExecutionContext context)
        {
            foreach (var onPart in sCriterion.SEntry.OnParts)
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

            if (sCriterion.SEntry.IfPart != null)
            {
                return ExpressionParser.IsValid(sCriterion.SEntry.IfPart.Condition, context);
            }

            return true;
        }
    }
}
