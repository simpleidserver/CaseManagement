using CaseManagement.CMMN;
using CaseManagement.CMMN.Infrastructure.Jobs.Notifications;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
{
    public static class MessageBrokerExtensions
    {
        public static Task QueueExternalEvent(this IMessageBroker messageBroker, string evtName, string casePlanInstanceId, string casePlanElementInstanceId, Dictionary<string, string> parameters, CancellationToken token)
        {
            return messageBroker.Queue(CMMNConstants.QueueNames.ExternalEvents, new ExternalEventNotification(Guid.NewGuid().ToString())
            {
                CasePlanElementInstanceId = casePlanElementInstanceId,
                CasePlanInstanceId = casePlanInstanceId,
                EvtName = evtName,
                Parameters = parameters
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
