﻿using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public interface IQueueProvider
    {
        Task Queue(string queueName, string message);
        Task<string> Dequeue(string queueName);
        Task<string> Peek(string queueName);
    }
}