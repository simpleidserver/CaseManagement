using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.MessageBroker
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

        public Task Queue<T>(string queueName, T message)
        {
            var genericType = typeof(T);
            var messageBrokerType = typeof(IMessageBrokerConsumerGeneric<>).MakeGenericType(genericType); 
            var lst = (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(messageBrokerType));
            foreach (var r in lst)
            {
                messageBrokerType.GetMethod("Handle").Invoke(r, new object[] { message });
            }

            return Task.CompletedTask;
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
