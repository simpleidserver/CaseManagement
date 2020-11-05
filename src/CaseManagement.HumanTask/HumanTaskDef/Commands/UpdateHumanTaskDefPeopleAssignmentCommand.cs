using CaseManagement.HumanTask.Common;
using MediatR;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefPeopleAssignmentCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public ICollection<AssignPeople> PeopleAssignments { get; set; }
    }
}
