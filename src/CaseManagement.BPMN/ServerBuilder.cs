using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Persistence;
using CaseManagement.BPMN.Persistence.InMemory;
using CaseManagement.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CaseManagement.BPMN
{
    public class ServerBuilder
    {
        private readonly IServiceCollection _services;

        public ServerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServerBuilder AddProcessInstances(ConcurrentBag<ProcessInstanceAggregate> processInstances)
        {
            _services.AddSingleton<IProcessInstanceQueryRepository>(new InMemoryProcessInstanceQueryRepository(processInstances));
            _services.AddSingleton<IProcessInstanceCommandRepository>(new InMemoryProcessInstanceCommandRepository(processInstances));
            var sp = _services.BuildServiceProvider();
            var commitAggregateHelper = sp.GetService<ICommitAggregateHelper>();
            foreach(var processInstance in processInstances)
            {
                commitAggregateHelper.Commit(processInstance, processInstance.GetStreamName(), CancellationToken.None).Wait();
            }

            return this;
        }

        public ServerBuilder AddProcessFiles(ConcurrentBag<ProcessFileAggregate> processFiles)
        {
            _services.AddSingleton<IProcessFileCommandRepository>(new InMemoryProcessFileCommandRepository(processFiles));
            _services.AddSingleton<IProcessFileQueryRepository>(new InMemoryProcessFileQueryRepository(processFiles));
            return this;
        }

        public ServerBuilder AddProcessFiles(List<string> lst)
        {
            var processFiles = new ConcurrentBag<ProcessFileAggregate>();
            foreach (var path in lst)
            {
                var bpmnTxt = File.ReadAllText(path);
                var name = Path.GetFileName(path);
                var processFile = ProcessFileAggregate.New(name, name, name, 0, bpmnTxt);
                processFiles.Add(processFile);
            }

            _services.AddSingleton<IProcessFileCommandRepository>(new InMemoryProcessFileCommandRepository(processFiles));
            _services.AddSingleton<IProcessFileQueryRepository>(new InMemoryProcessFileQueryRepository(processFiles));
            return this;
        }
    }
}
