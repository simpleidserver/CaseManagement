using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CasePlanElementInstanceTransitionHistory : ICloneable
    {
        public CasePlanElementInstanceTransitionHistory()
        {
            IncomingTokens = new Dictionary<string, string>();
        }

        public DateTime ExecutionDateTime { get; set; }
        public CMMNTransitions Transition { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> IncomingTokens { get; set; }

        public object Clone()
        {
            return new CasePlanElementInstanceTransitionHistory
            {
                ExecutionDateTime = ExecutionDateTime,
                Transition = Transition,
                Message = Message,
                IncomingTokens = IncomingTokens == null ? new Dictionary<string, string>() : IncomingTokens.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            };
        }
    }
}
