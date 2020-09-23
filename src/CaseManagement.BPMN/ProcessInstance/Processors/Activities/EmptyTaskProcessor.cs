using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class EmptyTaskProcessor : BaseActivityProcessor<EmptyTask>
    {
        protected override Task<ICollection<BaseToken>> Process(BPMNExecutionContext context, EmptyTask elt, CancellationToken cancellationToken)
        {
            ICollection<BaseToken> emptyTokens = new List<BaseToken>();
            return Task.FromResult(emptyTokens);
        }
    }
}
