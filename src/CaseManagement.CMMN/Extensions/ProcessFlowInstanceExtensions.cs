namespace CaseManagement.CMMN.Domains
{
    public static class ProcessFlowInstanceExtensions
    {
        /*
        public static bool IsRepetitionRuleValid(this ProcessFlowInstance processFlowInstance, CMMNPlanItemDefinition planItem)
        {
            if(planItem.ActivationRule != CMMNActivationRuleTypes.Repetition || planItem.RepetitionRule == null)
            {
                return false;
            }
            
            if (planItem.EntryCriterions.Any() && !processFlowInstance.IsEntryCriterionValid(planItem))
            {
                return false;
            }

            if (planItem.RepetitionRule.Condition == null)
            {
                return true;
            }

            return ExpressionParser.IsValid(planItem.RepetitionRule.Condition.Body, processFlowInstance);
        }

        public static bool IsEntryCriterionValid(this ProcessFlowInstance processFlowInstance, CMMNPlanItemDefinition planItem)
        {
            return planItem.EntryCriterions.Any(p => processFlowInstance.IsCriterionValid(p, planItem.Version));
        }

        public static bool IsCriterionValid(this ProcessFlowInstance processFlowInstance, CMMNCriterion criterion, int version)
        {
            foreach (var planItemOnPart in criterion.SEntry.PlanItemOnParts)
            {
                if (!string.IsNullOrWhiteSpace(planItemOnPart.SourceRef))
                {
                    var elt = processFlowInstance.GetPlanItem(planItemOnPart.SourceRef);
                    var transitionHistories = elt.TransitionHistories.Where(t => t.Version == version);
                    if (elt == null || !transitionHistories.Any() || transitionHistories.Any() && transitionHistories.Last().Transition != planItemOnPart.StandardEvent)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        */
    }
}