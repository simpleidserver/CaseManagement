using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors
{
    public class ServiceTaskProcessor : BaseActivityProcessor<ServiceTask>
    {
        private readonly IEnumerable<IServiceTaskHandler> _handlers;

        public ServiceTaskProcessor(IEnumerable<IServiceTaskHandler> handlers)
        {
            _handlers = handlers;
        }

        protected override async Task<ICollection<BaseToken>> Process(BPMNExecutionContext context, ServiceTask elt, CancellationToken cancellationToken)
        {
            var handler = _handlers.First(_ => _.Implementation == elt.Implementation);
            var result = await handler.Execute(context, elt, cancellationToken);
            return result;
        }
    }
}
