using CaseManagement.CMMN.Infrastructures.Bus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN
{
    public class CaseJobServer
    {
        private readonly Action<CMMNServerOptions> _action;
        private readonly IServiceCollection _serviceCollection;
        private IEnumerable<IMessageConsumer> _messageConsumers;

        public CaseJobServer(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public CaseJobServer(IServiceCollection serviceCollection, Action<CMMNServerOptions> action = null)
        {
            _serviceCollection = serviceCollection;
            _action = action;
        }

        public void Start()
        {
            if (_action != null)
            {
                _serviceCollection.AddCMMNEngine(_action);
            }
            else
            {
                _serviceCollection.AddCMMNEngine(act => { });
            }

            _serviceCollection.AddLogging();
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            _messageConsumers = serviceProvider.GetServices<IMessageConsumer>();
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Start();
            }
        }

        public void Stop()
        {
            foreach(var messageConsumer in _messageConsumers)
            {
                messageConsumer.Stop();
            }
        }
    }
}
