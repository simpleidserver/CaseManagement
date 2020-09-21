using CaseManagement.BPMN.Domains;
using CaseManagement.Common.Processors;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public abstract class BaseCatchEventProcessor<T> : BaseFlowNodeProcessor<T> where T : BaseCatchEvent
    {
        protected override Task Handle(ExecutionContext<ProcessInstanceAggregate> executionContext, T elt, CancellationToken cancellationToken)
        {
            if (elt.EventDefinitions.Any())
            {
                // TODO : Check the event definitions.
                return Task.CompletedTask;
            }

            executionContext.Instance.MakeTransition(elt, BPMNTransitions.COMPLETE);
            return Task.CompletedTask;
        }
    }
}
