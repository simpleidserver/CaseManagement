using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class DeleteNotificationDefPresentationParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
