using CaseManagement.Common.Domains;
using CaseManagement.Common.Jobs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.Bus
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

    }
}
