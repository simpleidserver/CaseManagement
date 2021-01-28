using CaseManagement.HumanTask.NotificationDef.Results;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.NotificationDef.Queries
{
    public class GetAllNotificationDefQuery : IRequest<ICollection<NotificationDefResult>>
    {
    }
}
