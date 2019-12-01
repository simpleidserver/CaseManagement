using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public abstract class BaseCMMNTaskProcessor : ICMMNPlanItemDefinitionProcessor
    {
        public abstract Type PlanItemDefinitionType { get; }

        public async Task<bool> Handle(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            var processTask = cmmnPlanItem.PlanItemDefinition as CMMNTask;
            if (cmmnPlanItem.ExitCriterions.Any() && cmmnPlanItem.ExitCriterions.Any(s => CheckCriterion(s, pf)))
            {
                cmmnPlanItem.Terminate();
                return true;
            }

            if (processTask.State == CMMNTaskStates.Available)
            {
                if (cmmnPlanItem.EntryCriterions.Any() && cmmnPlanItem.EntryCriterions.All(s => !CheckCriterion(s, pf)))
                {
                    return false;
                }

                if (cmmnPlanItem.PlanItemControl != null)
                {
                    var manualActivationRule = cmmnPlanItem.PlanItemControl as CMMNManualActivationRule;
                    if (manualActivationRule != null)
                    {
                        // Note : at the moment the ContextRef is ignored.
                        if (ExpressionParser.IsValid(manualActivationRule.Expression.Body, pf))
                        {
                            cmmnPlanItem.Enable();
                            return false;
                        }
                    }
                }

                cmmnPlanItem.Start();
            }

            if (processTask.State == CMMNTaskStates.Active)
            {
                return await Run(cmmnPlanItem, pf);
            }

            return true;
        }

        public abstract Task<bool> Run(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf);

        private bool CheckCriterion(CMMNCriterion sCriterion, ProcessFlowInstance pf)
        {
            foreach (var onPart in sCriterion.SEntry.OnParts)
            {
                if (onPart is CMMNPlanItemOnPart)
                {
                    if (!string.IsNullOrWhiteSpace(onPart.SourceRef))
                    {
                        var elt = pf.GetPlanItem(onPart.SourceRef);
                        if (elt == null || elt.Events.Last().Transition != onPart.StandardEvent)
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
