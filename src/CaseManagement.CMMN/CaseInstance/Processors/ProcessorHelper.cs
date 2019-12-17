using CaseManagement.CMMN.Domains;
using CaseManagement.Workflow.Domains;
using CaseManagement.Workflow.Engine;
using System.Linq;

namespace CaseManagement.CMMN.CaseInstance.Processors
{
    public class ProcessorHelper : IProcessorHelper
    {
        public RepetitionRuleResultTypes? HandleRepetitionRule(CMMNPlanItem cmmnPlanItem, ProcessFlowInstance pf)
        {
            var oneOccurence = false;
            int actualVersion = cmmnPlanItem.Version;
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
                                pf.CreatePlanItem(cmmnPlanItem);
                                // cmmnPlanItem.Create();
                                isNewInstance = true;
                            }
                        }
                        else
                        {
                            pf.CreatePlanItem(cmmnPlanItem);
                            // cmmnPlanItem.Create();
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
                            pf.CreatePlanItem(cmmnPlanItem);
                            isNewInstance = true;
                        }
                    }
                }

                if (!isNewInstance)
                {
                    if (!oneOccurence)
                    {
                        return RepetitionRuleResultTypes.Complete;
                    }
                }
            }

            if (!oneOccurence)
            {
                return RepetitionRuleResultTypes.Repeat;
            }

            return null;
        }
        
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
    }
}
