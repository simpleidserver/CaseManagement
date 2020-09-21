using CaseManagement.Common.Domains;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Processors
{
    public class ProcessorFactory : IProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute<TInstance, TElt>(ExecutionContext<TInstance> executionContext, TElt instance, CancellationToken token) where TInstance : BaseAggregate
        {
            var genericType = typeof(IProcessor<,>).MakeGenericType(typeof(TInstance), instance.GetType());
            var processor = _serviceProvider.GetService(genericType);
            return (Task)genericType.GetMethod("Execute").Invoke(processor, new object[] { executionContext, instance, token });
        }
    }
}
