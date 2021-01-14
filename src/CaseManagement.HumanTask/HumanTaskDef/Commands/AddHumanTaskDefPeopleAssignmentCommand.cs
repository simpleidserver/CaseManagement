using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefPeopleAssignmentCommand : IRequest<string>
    {
        public string Id { get; set; }
        public PeopleAssignmentDefinitionResult PeopleAssignment { get; set; }
    }
}
