using CaseManagement.HumanTask.NotificationInstance.Queries.Results;
using MediatR;

namespace CaseManagement.HumanTask.NotificationInstance.Queries
{
    public class GetNotificationsDetailsQuery : IRequest<NotificationDetailsResult>
    {
        public string NotificationId { get; set; }
    }
}