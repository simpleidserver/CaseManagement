using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class DeleteHumanTaskDefPeopleAssignmentCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string AssignmentId { get; set; }
    }
}
