using CaseManagement.BPMN.Common;
using CaseManagement.Common.Processors;
using System.Collections.Generic;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNExecutionResult : ExecutionResult
    {
        protected BPMNExecutionResult(bool isNext = false, bool isBlocked = false, object outcome = null, bool isEltInstanceCompleted = true, bool isNewExecutionPointerRequired = false) : base(isNext, isBlocked, outcome)
        {
            IsEltInstanceCompleted = isEltInstanceCompleted;
            IsNewExecutionPointerRequired = isNewExecutionPointerRequired;

        }

        public bool IsEltInstanceCompleted { get; set; }
        public bool IsNewExecutionPointerRequired { get; set; }

        public ICollection<BaseToken> Tokens => OutcomeValue as ICollection<BaseToken>;

        public static new BPMNExecutionResult Next()
        {
            return new BPMNExecutionResult(isNext: true);
        }

        public static new BPMNExecutionResult Block()
        {
            return new BPMNExecutionResult(isBlocked: true);
        }

        public static BPMNExecutionResult Outcome(ICollection<BaseToken> tokens, bool isEltInstanceCompleted = true,  bool isNewExecutionPointerRequired = false)
        {
            return new BPMNExecutionResult(isNext: true, outcome: tokens, isEltInstanceCompleted: isEltInstanceCompleted, isNewExecutionPointerRequired: isNewExecutionPointerRequired);
        }
    }
}
