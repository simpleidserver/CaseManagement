using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities
{
    public class EmptyTaskProcessor : BaseActivityProcessor<EmptyTask>
    {
        protected override Task<ICollection<MessageToken>> Process(BPMNExecutionContext context, EmptyTask elt, CancellationToken cancellationToken)
        {
            ICollection<MessageToken> result = new List<MessageToken> { MessageToken.EmptyMessage(context.Pointer.InstanceFlowNodeId, elt.EltId) };
            return Task.FromResult(result);
        }
    }
}
