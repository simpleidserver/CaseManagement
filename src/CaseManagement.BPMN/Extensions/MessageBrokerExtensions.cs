using CaseManagement.BPMN;
using CaseManagement.BPMN.Infrastructure.Jobs.Notifications;
using Newtonsoft.Json.Linq;
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

        public static Task QueueMessage(this IMessageBroker messageBroker, string processInstanceId, string messageName, object content, CancellationToken token)
        {
            return messageBroker.Queue(BPMNConstants.QueueNames.Messages, new MessageNotification(Guid.NewGuid().ToString())
            {
                ProcessInstanceId = processInstanceId,
                MessageName = messageName,
                Content = content
            }, token);
        }

        public static Task QueueStateTransition(this IMessageBroker messageBroker, string processInstanceId, string flowNodeInstanceId, string state, JObject jObj, CancellationToken token)
        {
            return messageBroker.Queue(BPMNConstants.QueueNames.StateTransitions, new StateTransitionNotification(Guid.NewGuid().ToString())
            {
                Content = jObj,
                State = state,
                ProcessInstanceId = processInstanceId,
                FlowNodeInstanceId = flowNodeInstanceId
            }, token);
        }
    }
}