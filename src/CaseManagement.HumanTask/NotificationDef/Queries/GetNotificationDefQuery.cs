using CaseManagement.HumanTask.NotificationDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Queries
{
    public class GetNotificationDefQuery : IRequest<NotificationDefResult>
    {
        public string Id { get; set; }
    }
}
