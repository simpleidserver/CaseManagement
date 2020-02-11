using CaseManagement.CMMN.Infrastructures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CaseManagement.CMMN
{
    public class CaseJobServer
    {
        private readonly Action<CMMNServerOptions> _action;
        private readonly IServiceCollection _serviceCollection;
        private IEnumerable<IJob> _jobs;

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
            _jobs = serviceProvider.GetServices<IJob>();
            foreach(var messageConsumer in _jobs)
            {
                messageConsumer.Start();
            }
        }

        public void Stop()
        {
            foreach(var messageConsumer in _jobs)
            {
                messageConsumer.Stop();
            }
        }
    }
}
