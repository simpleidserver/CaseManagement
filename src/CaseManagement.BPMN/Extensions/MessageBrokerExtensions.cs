using CaseManagement.BPMN;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueProcessInstance(this IMessageBroker messageBroker, string processInstanceId, bool isNewInstance, CancellationToken token)
        {
            return messageBroker.Queue(BPMNConstants.QueueNames.ProcessInstances, new ProcessInstanceNotification(Guid.NewGuid().ToString())
            {
                ProcessInstanceId = processInstanceId,
                IsNewInstance = isNewInstance
            }, token);
        }

        public static Task QueueMessage(this IMessageBroker messageBroker, string processInstanceId, string messageName, CancellationToken token)
        {
            return messageBroker.Queue(BPMNConstants.QueueNames.Messages, new MessageNotification(Guid.NewGuid().ToString())
            {
                ProcessInstanceId = processInstanceId,
                MessageName = messageName
            }, token);
        }
    }
}