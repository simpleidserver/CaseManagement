using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.BPMN.Helpers;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Resources;
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
        private readonly IDelegateConfigurationRepository _delegateConfigurationRepository;

        public CallbackServiceTaskHandler(
            IServiceProvider serviceProvider,
            IDelegateConfigurationRepository delegateConfigurationRepository)
        {
            _serviceProvider = serviceProvider;
            _delegateConfigurationRepository = delegateConfigurationRepository;
        }

        public string Implementation { get => BPMNConstants.ServiceTaskImplementations.CALLBACK; }

        public async Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ServiceTask serviceTask, CancellationToken cancellationToken)
        {
            var configuration = await _delegateConfigurationRepository.Get(serviceTask.DelegateId, cancellationToken);
            if (configuration == null)
            {
                throw new BPMNProcessorException(string.Format(Global.UnknownDelegate, serviceTask.DelegateId));
            }

            var type = TypeResolver.ResolveType(configuration.FullQualifiedName);
            var handler = (IDelegateHandler)_serviceProvider.GetService(type);
            return await handler.Execute(context.Pointer.Incoming.ToList(), configuration, cancellationToken);
        }
    }
}
