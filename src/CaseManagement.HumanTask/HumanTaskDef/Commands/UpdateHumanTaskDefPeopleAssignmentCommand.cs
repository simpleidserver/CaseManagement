using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefPeopleAssignmentCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public HumanTaskDefAssignmentResult PeopleAssignment { get; set; }
    }
}
