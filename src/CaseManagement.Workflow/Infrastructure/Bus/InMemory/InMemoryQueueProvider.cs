﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus.InMemory
{
    public class InMemoryQueueProvider : IQueueProvider
    {
        private Dictionary<string, BlockingCollection<string>> _queues = new Dictionary<string, BlockingCollection<string>>();

        public InMemoryQueueProvider()
        {
            _queues = new Dictionary<string, BlockingCollection<string>>();
        }

        public Task Queue(string queueName, string message)
        {
            lock (_queues)
            {
                if (!_queues.ContainsKey(queueName))
                {
                    _queues.Add(queueName, new BlockingCollection<string> { message });
                    return Task.CompletedTask;
                }

                _queues[queueName].Add(message);
                return Task.CompletedTask;
            }
        }

        public Task<string> Dequeue(string queueName)
        {
            lock(_queues)
            {
                string json;
                if (!_queues.ContainsKey(queueName))
                {
                    return Task.FromResult<string>(null);
                }

                if (_queues[queueName].TryTake(out json))
                {
                    return Task.FromResult(json);
                }

                return Task.FromResult<string>(null);
            }
        }

        public Task<string> Peek(string queueName)
        {
            lock (_queues)
            {
                if (!_queues.ContainsKey(queueName))
                {
                    return Task.FromResult<string>(null);
                }

                if (_queues[queueName].Count == 0)
                {
                    return Task.FromResult<string>(null);
                }

                return Task.FromResult(_queues[queueName].First());
            }
        }
    }
}