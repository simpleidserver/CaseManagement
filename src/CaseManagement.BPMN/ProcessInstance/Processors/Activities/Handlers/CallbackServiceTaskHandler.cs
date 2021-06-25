using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities.Handlers
{
    public class CallbackServiceTaskHandler : IServiceTaskHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public CallbackServiceTaskHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string Implementation { get => BPMNConstants.ServiceTaskImplementations.CALLBACK; }

        public Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ServiceTask serviceTask, CancellationToken cancellationToken)
        {
            var type = TypeResolver.ResolveType(serviceTask.ClassName);
            var handler = (IDelegateHandler)_serviceProvider.GetService(type);
            return handler.Execute(context.Pointer.Incoming.ToList(), cancellationToken);
        }
    }
}
