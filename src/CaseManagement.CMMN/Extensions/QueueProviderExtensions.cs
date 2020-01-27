using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Bus.ConfirmForm;
using CaseManagement.CMMN.Infrastructures.Bus.ConfirmTableItem;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeDomainEvent;
using CaseManagement.CMMN.Infrastructures.Bus.ConsumeTransitionEvent;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchProcess;
using CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public static class QueueProviderExtensions
    {
        public static Task QueueEvent(this IQueueProvider queueProvider, DomainEvent domainEvent)
        {
            var message = new DomainEventMessage { AssemblyQualifiedName = domainEvent.GetType().AssemblyQualifiedName, Content = JsonConvert.SerializeObject(domainEvent) };
            return queueProvider.Queue(DomainEventMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueTransition(this IQueueProvider queueProvider, string caseInstanceId, string caseInstanceElementId, CMMNTransitions transition)
        {
            var message = new TransitionEventMessage { Id = Guid.NewGuid().ToString(), CaseInstanceId = caseInstanceId, CaseInstanceElementId = caseInstanceElementId, Transition = transition };
            return queueProvider.Queue(TransitionEventMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueLaunchProcess(this IQueueProvider queueProvider, string processId)
        {
            var message = new LaunchProcessMessage(processId);
            return queueProvider.Queue(LaunchProcessMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueReactivateProcess(this IQueueProvider queueProvider, string caseInstanceId)
        {
            var message = new ReactivateProcessMessage(caseInstanceId);
            return queueProvider.Queue(ReactivateProcessMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueSubmitForm(this IQueueProvider queueProvider, string caseInstanceId, string caseElementInstanceId, string formInstanceId, Dictionary<string, string> formValues)
        {
            var message = new ConfirmFormMessage(caseInstanceId, caseElementInstanceId, formInstanceId, formValues);
            return queueProvider.Queue(ConfirmFormMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }

        public static Task QueueConfirmTableItem(this IQueueProvider queueProvider, string caseInstanceId, string caseElementDefinitionId, string user)
        {
            var message = new ConfirmTableItemMessage(caseInstanceId, caseElementDefinitionId, user);
            return queueProvider.Queue(ConfirmFormMessageConsumer.QUEUE_NAME, JsonConvert.SerializeObject(message));
        }
    }
}
