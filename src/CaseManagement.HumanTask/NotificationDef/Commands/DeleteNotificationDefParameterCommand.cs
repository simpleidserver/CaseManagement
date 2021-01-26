using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class DeleteNotificationDefParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string ParameterId { get; set; }
    }
}
