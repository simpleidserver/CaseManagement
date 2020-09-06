using CaseManagement.CMMN.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class ProcessorFactory : IProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute<T>(ExecutionContext executionContext, T instance, CancellationToken token) where T : CasePlanElementInstance
        {
            return Execute(executionContext, instance, typeof(T), token);
        }

        public Task Execute(ExecutionContext executionContext, CasePlanElementInstance instance, Type type, CancellationToken token)
        {
            var genericType = typeof(BaseProcessor<>).MakeGenericType(type);
            var processor = _serviceProvider.GetService(genericType);
            return (Task)genericType.GetMethod("Execute").Invoke(processor, new object[] { executionContext, instance, token });
        }
    }
}
