using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class DeleteNotificationDefCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
