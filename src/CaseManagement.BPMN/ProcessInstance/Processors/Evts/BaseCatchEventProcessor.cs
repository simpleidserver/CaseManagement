using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Evts
{
    public abstract class BaseCatchEventProcessor<T> : BaseFlowNodeProcessor<T> where T : BaseCatchEvent
    {
        protected override Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, T nodeDef, CancellationToken cancellationToken)
        {
            var flowNodeIds = GetNextFlowNodeIds(executionContext, nodeDef);
            if (nodeDef.EventDefinitions.Any())
            {
                var lst = new List<bool>();
                foreach(var evtDef in nodeDef.EventDefinitions)
                {
                    lst.Add(executionContext.Pointer.Incoming.Any(_ => evtDef.IsSatisfied(executionContext.Instance, _)));
                }

                if ((nodeDef.ParallelMultiple && lst.All(_ => _ == true)) ||
                    (!nodeDef.ParallelMultiple && lst.Any(_ => _ == true)))
                {
                    var outcome = new List<MessageToken>();
                    outcome.AddRange(executionContext.Pointer.Incoming);
                    return Task.FromResult(BPMNExecutionResult.Next(flowNodeIds, executionContext.Pointer.Incoming.ToList(), isEltInstanceCompleted: false, isNewExecutionPointerRequired: true));
                }

                return Task.FromResult(BPMNExecutionResult.Block());
            }

            return Task.FromResult(BPMNExecutionResult.Next(flowNodeIds, new List<MessageToken>() { MessageToken.EmptyMessage() }));
        }
    }
}
