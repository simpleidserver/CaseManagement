using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CaseFileItemInstance : CasePlanElementInstance
    {
        public CaseFileItemStates? State { get; set; }
        public string DefinitionType { get; set; }
        public override string Type { get => "fileitem"; }

        public override object Clone()
        {
            var result = new CaseFileItemInstance
            {
                DefinitionType = DefinitionType,
                State = State
            };
            FeedCasePlanElement(result);
            return result;
        }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            var newState = GetCaseFileItemState(State, transition);
            if (newState != null)
            {
                State = newState;
            }
        }
    }
}