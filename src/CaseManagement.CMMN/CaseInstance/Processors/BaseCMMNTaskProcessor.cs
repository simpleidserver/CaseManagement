using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : IProcessFlowElementProcessor
    {
        public abstract string ProcessFlowElementType { get; }

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            context.Start(token);
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var processTask = ExtractTask(cmmnPlanItem);
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
                    pf.BlockElement(cmmnPlanItem);
                    return;
                }

                if (cmmnPlanItem.ActivationRule != null)
                {
                    switch(cmmnPlanItem.ActivationRule)
                    {
                        case CMMNActivationRuleTypes.ManualActivation:
                            // Note : at the moment the ContextRef is ignored.
                            if (ExpressionParser.IsValid(cmmnPlanItem.ManualActivationRule.Expression.Body, pf))
                            {
                                pf.EnablePlanItem(cmmnPlanItem);
                                return;
                            }
                            break;
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

        public static bool CheckCriterion(CMMNCriterion sCriterion, ProcessFlowInstance pf)
        {
            foreach (var planItemOnPart in sCriterion.SEntry.PlanItemOnParts)
            {
                if (!string.IsNullOrWhiteSpace(planItemOnPart.SourceRef))
                {
                    var elt = pf.GetPlanItem(planItemOnPart.SourceRef);
                    if (elt == null || elt.TransitionHistories.Any() && elt.TransitionHistories.Last().Transition != planItemOnPart.StandardEvent)
                    {
                        return false;
                    }
                }
            }
            
            foreach (var fileItemOnPart in sCriterion.SEntry.FileItemOnParts)
            {
                if (!string.IsNullOrWhiteSpace(fileItemOnPart.SourceRef))
                {
                    var elt = pf.GetCaseFileItem(fileItemOnPart.SourceRef);
                    if (elt == null || elt.TransitionHistories.Any() && elt.TransitionHistories.Last().Transition != fileItemOnPart.StandardEvent)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static CMMNTask ExtractTask(CMMNPlanItem planItem)
        {
            switch(planItem.PlanItemDefinitionType)
            {
                case CMMNPlanItemDefinitionTypes.HumanTask:
                    return planItem.PlanItemDefinitionHumanTask;
                case CMMNPlanItemDefinitionTypes.ProcessTask:
                    return planItem.PlanItemDefinitionProcessTask;
                case CMMNPlanItemDefinitionTypes.Task:
                    return planItem.PlanItemDefinitionTask;
            }

            return null;
        }
    }
}
