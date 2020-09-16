using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.Jobs;
using CaseManagement.CMMN.Infrastructure.Jobs.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Infrastructure.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueDomainEvents(this IMessageBroker messageBroker, ICollection<DomainEvent> domainEvts, CancellationToken token)
        {
            return messageBroker.Queue("domainevts", new DomainEventNotification(Guid.NewGuid().ToString())
            {
                Evts = domainEvts.Select(_ =>
                new DomainEventNotificationRecord
                {
                    Content = JsonConvert.SerializeObject(_),
                    Type = _.GetType().FullName
                }).ToList()
            }, token);
        }

        public static Task QueueExternalEvent(this IMessageBroker messageBroker, string evtName, string casePlanInstanceId, string casePlanElementInstanceId, CancellationToken token)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.ExternalEvents, new ExternalEventNotification(Guid.NewGuid().ToString())
            {
                CasePlanElementInstanceId = casePlanElementInstanceId,
                CasePlanInstanceId = casePlanInstanceId,
                EvtName = evtName
            }, token);
        }

        public static Task QueueCasePlanInstance(this IMessageBroker messageBroker, string casePlanInstanceId, CancellationToken token)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.CasePlanInstances, new CasePlanInstanceNotification(Guid.NewGuid().ToString())
            {
                CasePlanInstanceId = casePlanInstanceId
            }, token);
        }
        public static Task ScheduleExternalEvt(this IMessageBroker messageBroker, string evtName, string casePlanInstanceId, string casePlanElementInstanceId, DateTime elapsedTime, CancellationToken token)
        {
            return messageBroker.QueueScheduledMessage(CMMNConstants.QueueNames.ExternalEvents, new ExternalEventNotification(Guid.NewGuid().ToString())
            {
                CasePlanElementInstanceId = casePlanElementInstanceId,
                CasePlanInstanceId = casePlanInstanceId,
                EvtName = evtName
            }, elapsedTime, token);
        }
    }
}
