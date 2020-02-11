using CaseManagement.CMMN;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Bus.LaunchCasePlanInstance;
using CaseManagement.CMMN.Infrastructures.Bus.ReactivateProcess;
using CaseManagement.CMMN.Infrastructures.Bus.Transition;
using System;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueEvent(this IMessageBroker messageBroker, DomainEvent domainEvent, string queueName)
        {
            return messageBroker.Queue(queueName, domainEvent);
        }

        public static Task QueueTransition(this IMessageBroker messageBroker, string casePlanInstanceId, string casePlanElementInstanceId, CMMNTransitions transition)
        {
            var message = new TransitionCommand { Id = Guid.NewGuid().ToString(), CasePlanInstanceId = casePlanInstanceId, CasePlanElementInstanceId = casePlanElementInstanceId, Transition = transition };
            return messageBroker.Queue(CMMNConstants.QueueNames.CasePlanInstances, message);
        }

        public static Task QueueLaunchProcess(this IMessageBroker messageBroker, string casePlanInstanceId)
        {
            var message = new LaunchCasePlanInstanceCommand(casePlanInstanceId);
            return messageBroker.Queue(CMMNConstants.QueueNames.CasePlanInstances, message);
        }

        public static Task QueueReactivateProcess(this IMessageBroker messageBroker, string caseInstanceId)
        {
            var message = new ReactivateProcessCommand(caseInstanceId);
            return messageBroker.Queue(CMMNConstants.QueueNames.CasePlanInstances, message);
        }
    }
}
