using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class EmptyTaskProcessor : BaseActivityProcessor<EmptyTask>
    {
        protected override Task Process(ExecutionContext<ProcessInstanceAggregate> context, EmptyTask elt, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
