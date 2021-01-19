using MediatR;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class DeleteNotificationDefPeopleAssignmentCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
    }
}
