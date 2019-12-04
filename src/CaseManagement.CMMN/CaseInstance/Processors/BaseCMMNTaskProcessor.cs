using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : IProcessFlowElementProcessor
    {
        public abstract Type ProcessFlowElementType { get; }

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start();
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var processTask = context.GetCMMNTask();
            if (cmmnPlanItem.ExitCriterions.Any() && cmmnPlanItem.ExitCriterions.Any(s => CheckCriterion(s, pf)))
            {
                pf.TerminatePlanItem(cmmnPlanItem.Id);
                await context.Complete(token);
                return;
            }

            if (processTask.State == CMMNTaskStates.Available)
            {
                if (cmmnPlanItem.EntryCriterions.Any() && cmmnPlanItem.EntryCriterions.All(s => !CheckCriterion(s, pf)))
                {
                    // TODO : MANAGE BLOCKED ELEMENT.
                    return;
                }

                if (cmmnPlanItem.PlanItemControl != null)
                {
                    var manualActivationRule = cmmnPlanItem.PlanItemControl as CMMNManualActivationRule;
                    if (manualActivationRule != null)
                    {
                        // Note : at the moment the ContextRef is ignored.
                        if (ExpressionParser.IsValid(manualActivationRule.Expression.Body, pf))
                        {
                            pf.EnablePlanItem(cmmnPlanItem);
                            return;
                        }
                    }
                }

                pf.StartPlanItem(cmmnPlanItem);
            }

            if (processTask.State == CMMNTaskStates.Active)
            {
                await Run(context, token);
                return;
            }

            await context.Complete(token);
        }

        public abstract Task Run(WorkflowHandlerContext context, CancellationToken token);

        private bool CheckCriterion(CMMNCriterion sCriterion, ProcessFlowInstance pf)
        {
            foreach (var onPart in sCriterion.SEntry.OnParts)
            {
                if (onPart is CMMNPlanItemOnPart)
                {
                    if (!string.IsNullOrWhiteSpace(onPart.SourceRef))
                    {
                        var elt = pf.GetPlanItem(onPart.SourceRef);
                        if (elt == null || elt.TransitionHistories.Last().Transition != onPart.StandardEvent)
                        {
                            return false;
                        }
                    }
                }
            }

            if (sCriterion.SEntry.IfPart != null)
            {
                return ExpressionParser.IsValid(sCriterion.SEntry.IfPart.Condition, pf);
            }

            return true;
        }
    }
}
