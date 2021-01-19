using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class UpdateNotificationDefInfoCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}
