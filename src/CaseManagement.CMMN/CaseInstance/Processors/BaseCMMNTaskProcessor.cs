using CaseManagement.CMMN.CaseInstance.Watchers;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : IProcessFlowElementProcessor
    {
        public BaseCMMNTaskProcessor(IDomainEventWatcher domainEventWatcher)
        {
            DomainEventWatcher = domainEventWatcher;
        }

        public abstract string ProcessFlowElementType { get; }
        protected IDomainEventWatcher DomainEventWatcher { get; private set; }

        public async Task Handle(WorkflowHandlerContext context, CancellationToken token)
        {
            bool oneOccurence = false;
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var processTask = ExtractTask(cmmnPlanItem);
            int actualVersion = cmmnPlanItem.Version;
            if (cmmnPlanItem.Status == null)
            {
                context.Start(token);
                DomainEventWatcher.AddCallback(async (obj, e) =>
                {
                    var evt = e.DomainEvent as ProcessFlowInstanceElementStateChangedEvent;
                    if (evt == null)
                    {
                        return;
                    }

                    var name = Enum.GetName(typeof(CMMNPlanItemTransitions), CMMNPlanItemTransitions.Terminate);
                    if (!evt.State.Equals(name, StringComparison.InvariantCultureIgnoreCase) || evt.ElementId != context.CurrentElement.Id)
                    {
                        return;
                    }

                    Complete(context);
                    await context.ExecuteNext(token);
                    return;
                });
                await context.StartSubProcess(DomainEventWatcher, token);
            }
            else
            {
                if (cmmnPlanItem.RepetitionRule != null && cmmnPlanItem.ActivationRule == CMMNActivationRuleTypes.Repetition)
                {
                    bool isNewInstance = false;
                    if (cmmnPlanItem.EntryCriterions.Any())
                    {
                        oneOccurence = true;
                        if (cmmnPlanItem.EntryCriterions.Any(s => CheckCriterion(s, pf, actualVersion + 1)))
                        {
                            if (cmmnPlanItem.RepetitionRule.Condition != null)
                            {
                                if (ExpressionParser.IsValid(cmmnPlanItem.RepetitionRule.Condition.Body, pf))
                                {
                                    cmmnPlanItem.Create();
                                    isNewInstance = true;
                                }
                            }
                            else
                            {
                                cmmnPlanItem.Create();
                                isNewInstance = true;
                            }
                        }
                    }
                    else
                    {
                        if (cmmnPlanItem.RepetitionRule.Condition != null)
                        {
                            if (ExpressionParser.IsValid(cmmnPlanItem.RepetitionRule.Condition.Body, pf))
                            {
                                cmmnPlanItem.Create();
                                isNewInstance = true;
                            }
                        }
                    }

                    if (!isNewInstance)
                    { 
                        if (!oneOccurence)
                        {
                            Complete(context);
                        }
                        return;
                    }
                }
            }

            if (cmmnPlanItem.Status == ProcessFlowInstanceElementStatus.Blocked)
            {
                return;
            }

            if (cmmnPlanItem.EntryCriterions.Any() && cmmnPlanItem.EntryCriterions.All(s => !CheckCriterion(s, pf, actualVersion)))
            {
                return;
            }

            // Available => Enabled
            if (cmmnPlanItem.ActivationRule != null && cmmnPlanItem.ActivationRule == CMMNActivationRuleTypes.ManualActivation)
            {
                // NOTE : at the moment the ContextRef is ignored.
                if (cmmnPlanItem.ManualActivationRule.Expression == null || ExpressionParser.IsValid(cmmnPlanItem.ManualActivationRule.Expression.Body, pf))
                {
                    pf.CreateManualStart(cmmnPlanItem);
                    DomainEventWatcher.AddCallback(async (obj, e) =>
                    {
                        var evt = e.DomainEvent as ProcessFlowInstanceElementStateChangedEvent;
                        if (evt == null)
                        {
                            return;
                        }

                        var name = Enum.GetName(typeof(CMMNPlanItemTransitions), CMMNPlanItemTransitions.ManualStart);
                        if (!evt.State.Equals(name, StringComparison.InvariantCultureIgnoreCase) || evt.ElementId != context.CurrentElement.Id)
                        {
                            return;
                        }

                        await InternalHandle(context, token, oneOccurence);
                        DomainEventWatcher.Quit = true;
                    });
                    return;
                }
            }

            await InternalHandle(context, token, oneOccurence);
        }

        public abstract Task Run(WorkflowHandlerContext context, CancellationToken token);

        public static bool CheckCriterion(CMMNCriterion sCriterion, ProcessFlowInstance pf, int currentVersion)
        {
            foreach (var planItemOnPart in sCriterion.SEntry.PlanItemOnParts)
            {
                if (!string.IsNullOrWhiteSpace(planItemOnPart.SourceRef))
                {
                    var elt = pf.GetPlanItem(planItemOnPart.SourceRef);
                    var transitionHistories = elt.TransitionHistories.Where(t => t.Version == currentVersion);
                    if (elt == null || !transitionHistories.Any() || transitionHistories.Any() && transitionHistories.Last().Transition != planItemOnPart.StandardEvent)
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

        protected virtual void Complete(WorkflowHandlerContext context)
        {
            DomainEventWatcher.Quit = true;
            context.Complete();
        }

        private async Task InternalHandle(WorkflowHandlerContext context, CancellationToken token, bool oneOccurence)
        {
            var pf = context.ProcessFlowInstance;
            var cmmnPlanItem = context.GetCMMNPlanItem();
            var processTask = ExtractTask(cmmnPlanItem);
            int actualVersion = cmmnPlanItem.Version;
            // Available => Active
            pf.StartPlanItem(cmmnPlanItem);
            // Active => Terminated
            if (cmmnPlanItem.ExitCriterions.Any() && cmmnPlanItem.ExitCriterions.Any(s => CheckCriterion(s, pf, cmmnPlanItem.Version)))
            {
                pf.TerminatePlanItem(cmmnPlanItem.Id);
                context.Complete();
                await context.ExecuteNext(token);
                return;
            }

            // Active => Completed || Active => Failed || Active => Suspended
            if (processTask.State == CMMNTaskStates.Active)
            {
                await Run(context, token);
                if (cmmnPlanItem.ActivationRule != CMMNActivationRuleTypes.Repetition)
                {
                    Complete(context);
                }
                else if (!oneOccurence)
                {
                    await Handle(context, token);
                }

                return;
            }
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
