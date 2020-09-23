using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseCatchEventProcessor<T> : BaseFlowNodeProcessor<T> where T : BaseCatchEvent
    {
        protected override Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, T nodeDef, CancellationToken cancellationToken)
        {
            if (nodeDef.EventDefinitions.Any())
            {
                var lst = new List<bool>();
                foreach(var evtDef in nodeDef.EventDefinitions)
                {
                    lst.Add(executionContext.Pointer.Incoming.Any(_ => evtDef.IsSatisfied(_)));
                }

                if ((nodeDef.ParallelMultiple && lst.All(_ => _ == true)) ||
                    (!nodeDef.ParallelMultiple && lst.Any(_ => _ == true)))
                {
                    var outcome = new List<BaseToken>();
                    outcome.AddRange(executionContext.Pointer.Incoming);
                    return Task.FromResult(BPMNExecutionResult.Outcome(outcome, isEltInstanceCompleted: false, isNewExecutionPointerRequired: true));
                }

                return Task.FromResult(BPMNExecutionResult.Block());
            }

            return Task.FromResult(BPMNExecutionResult.Outcome(new List<BaseToken> { MessageToken.EmptyMessage() }));
        }
    }
}
