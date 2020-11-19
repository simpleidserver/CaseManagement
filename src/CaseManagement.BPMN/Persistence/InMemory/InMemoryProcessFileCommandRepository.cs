﻿using CaseManagement.BPMN.Domains;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Persistence.InMemory
{
    public class InMemoryProcessFileCommandRepository : IProcessFileCommandRepository
    {
        private readonly ConcurrentBag<ProcessFileAggregate> _processFiles;

        public InMemoryProcessFileCommandRepository(ConcurrentBag<ProcessFileAggregate> processFiles)
        {
            _processFiles = processFiles;
        }

        public Task Add(ProcessFileAggregate processFile, CancellationToken token)
        {
            _processFiles.Add((ProcessFileAggregate)processFile.Clone());
            return Task.CompletedTask;
        }

        public Task<int> SaveChanges(CancellationToken token)
        {
            return Task.FromResult(1);
        }
    }
}