using CaseManagement.HumanTask.NotificationDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class AddNotificationDefCommand : IRequest<NotificationDefResult>
    {
        public string Name { get; set; }
    }
}
