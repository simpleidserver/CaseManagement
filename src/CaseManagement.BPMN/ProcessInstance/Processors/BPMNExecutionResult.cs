using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Collections.Generic;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class BPMNExecutionResult : ExecutionResult
    {
        protected BPMNExecutionResult(
            bool isNext = false, 
            bool isBlocked = false, 
            object outcome = null, 
            bool isEltInstanceCompleted = true, 
            bool isNewExecutionPointerRequired = false, 
            ICollection<string> nextFlowNodeIds = null) : base(isNext, isBlocked, outcome)
        {
            IsEltInstanceCompleted = isEltInstanceCompleted;
            IsNewExecutionPointerRequired = isNewExecutionPointerRequired;
            NextFlowNodeIds = nextFlowNodeIds;
        }

        public bool IsEltInstanceCompleted { get; set; }
        public bool IsNewExecutionPointerRequired { get; set; }
        public ICollection<string> NextFlowNodeIds { get; set; }
        public ICollection<MessageToken> OutcomingTokens { get; set; }
        public IEnumerable<MessageToken> Tokens => OutcomeValue as IEnumerable<MessageToken>;

        public static BPMNExecutionResult Next(ICollection<string> nextFlowNodeIds, IEnumerable<MessageToken> outcome = null, bool isEltInstanceCompleted = true, bool isNewExecutionPointerRequired = false)
        {
            return new BPMNExecutionResult(isNext: true, nextFlowNodeIds: nextFlowNodeIds, outcome: outcome, isEltInstanceCompleted: isEltInstanceCompleted, isNewExecutionPointerRequired: isNewExecutionPointerRequired);
        }

        public static new BPMNExecutionResult Block()
        {
            return new BPMNExecutionResult(isBlocked: true);
        }
    }
}
