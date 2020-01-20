using CaseManagement.CMMN.Infrastructures.Bus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN
{
    public class CaseJobServer : IDisposable
    {
        private readonly Action<CMMNServerOptions> _action;
        private readonly Action<IServiceCollection> _serviceCallback;
        private IEnumerable<IMessageConsumer> _messageConsumers;

        public CaseJobServer(Action<IServiceCollection> serviceCallback = null)
        {
            _serviceCallback = serviceCallback;
            Start();
        }

        public CaseJobServer(Action<CMMNServerOptions> action = null, Action<IServiceCollection> serviceCallback = null)
        {
            _action = action;
            _serviceCallback = serviceCallback;
            Start();
        }

        private void Start()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            if (_action != null)
            {
                serviceCollection.AddCMMNEngine(_action);
            }
            else
            {
                serviceCollection.AddCMMNEngine(act => { });
            }

            if (_serviceCallback != null)
            {
                _serviceCallback(serviceCollection);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _messageConsumers = serviceProvider.GetServices<IMessageConsumer>();
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Start();
            }
        }

        public void Dispose()
        {
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Stop();
            }
        }
    }
}
