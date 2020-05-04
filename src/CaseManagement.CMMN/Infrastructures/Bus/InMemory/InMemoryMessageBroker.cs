using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructures.Bus.InMemory
{
    public class InMemoryMessageBroker : IMessageBroker
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryMessageBroker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public async Task Queue(string queueName, object message)
        {
            var genericType = message.GetType();
            var messageBrokerType = typeof(IMessageBrokerConsumerGeneric<>).MakeGenericType(genericType);
            var lst = (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(messageBrokerType));
            var lstTask = new List<Task>();
            foreach (var r in lst)
            {
                lstTask.Add((Task)messageBrokerType.GetMethod("Handle").Invoke(r, new object[] { message, CancellationToken.None }));
            }

            await Task.WhenAll(lstTask);
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
