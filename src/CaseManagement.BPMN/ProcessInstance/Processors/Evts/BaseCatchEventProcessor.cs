using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Evts
{
    public abstract class BaseCatchEventProcessor<T> : BaseFlowNodeProcessor<T> where T : BaseCatchEvent
    {
        protected override async Task<BPMNExecutionResult> Handle(BPMNExecutionContext executionContext, T nodeDef, CancellationToken cancellationToken)
        {
            await InternalHandle(executionContext, nodeDef, cancellationToken);
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
                    return BPMNExecutionResult.Next(flowNodeIds, executionContext.Pointer.Incoming.ToList(), isEltInstanceCompleted: false, isNewExecutionPointerRequired: true);
                }

                return BPMNExecutionResult.Block();
            }

            return BPMNExecutionResult.Next(flowNodeIds, new List<MessageToken> { MessageToken.EmptyMessage(executionContext.Pointer.InstanceFlowNodeId, nodeDef.EltId) });
        }

        protected abstract Task InternalHandle(BPMNExecutionContext executionContext, T nodeDef, CancellationToken cancellationToken);
    }
}
