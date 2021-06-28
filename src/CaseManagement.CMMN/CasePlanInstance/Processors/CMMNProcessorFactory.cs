using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNProcessorFactory : ICMMNProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CMMNProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<ExecutionResult> Execute(CMMNExecutionContext executionContext, CaseEltInstance instance, CancellationToken token)
        {
            var genericType = typeof(IEnumerable<ICaseEltInstanceProcessor>);
            var processors = (IEnumerable<ICaseEltInstanceProcessor>)_serviceProvider.GetService(genericType);
            var processor = processors.First(_ => _.Type == instance.Type);
            return processor.Execute(executionContext, instance, token);
        }
    }
}
