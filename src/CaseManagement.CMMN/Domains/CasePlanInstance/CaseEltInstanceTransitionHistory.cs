using System;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CaseEltInstanceTransitionHistory : ICloneable
    {
        public DateTime ExecutionDateTime { get; set; }
        public CMMNTransitions Transition { get; set; }
        public string Message { get; set; }

        public object Clone()
        {
            return new CaseEltInstanceTransitionHistory
            {
                ExecutionDateTime = ExecutionDateTime,
                Transition = Transition,
                Message = Message
            };
        }
    }
}
