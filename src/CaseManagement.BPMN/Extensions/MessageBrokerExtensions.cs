using CaseManagement.BPMN;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueProcessInstance(this IMessageBroker messageBroker, string processInstanceId, CancellationToken token)
        {
            return messageBroker.Queue(BPMNConstants.QueueNames.ProcessInstances, new ProcessInstanceNotification(Guid.NewGuid().ToString())
            {
                ProcessInstanceId = processInstanceId
            }, token);
        }
    }
}