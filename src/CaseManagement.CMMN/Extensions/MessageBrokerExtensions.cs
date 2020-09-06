using CaseManagement.CMMN;
using CaseManagement.CMMN.Infrastructures;
using CaseManagement.CMMN.Infrastructures.Bus;
using CaseManagement.CMMN.Infrastructures.Jobs.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Infrastructure.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueExternalEvent(this IMessageBroker messageBroker, string evtName, string casePlanInstanceId, string casePlanElementInstanceId)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.ExternalEvents, new ExternalEventNotification(Guid.NewGuid().ToString())
            {
                CasePlanElementInstanceId = casePlanElementInstanceId,
                CasePlanInstanceId = casePlanInstanceId,
                EvtName = evtName
            });
        }

        public static Task QueueCasePlanInstance(this IMessageBroker messageBroker, string casePlanInstanceId)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.CasePlanInstances, new CasePlanInstanceNotification(Guid.NewGuid().ToString())
            {
                CasePlanInstanceId = casePlanInstanceId
            });
        }

        public static Task QueueDomainEvents(this IMessageBroker messageBroker, ICollection<DomainEvent> domainEvts)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.DomainEvents, new DomainEventNotification(Guid.NewGuid().ToString())
            {
                Evts = domainEvts.Select(_ =>
                new DomainEventNotificationRecord
                {
                    Content = JsonConvert.SerializeObject(_),
                    Type = _.GetType().FullName
                }).ToList()
            });
        }
    }
}
