using MediatR;
using static CaseManagement.HumanTask.NotificationDef.Results.NotificationDefResult;

namespace CaseManagement.HumanTask.NotificationDef.Commands
{
    public class AddNotificationDefPeopleAssignmentCommand : IRequest<string>
    {
        public string Id { get; set; }
        public PeopleAssignmentDefinitionResult PeopleAssignment { get; set; }
    }
}
