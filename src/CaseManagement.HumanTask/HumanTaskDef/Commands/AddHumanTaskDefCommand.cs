using CaseManagement.HumanTask.HumanTaskDef.Results;
using MediatR;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefCommand : IRequest<HumanTaskDefResult>
    {
        public string Name { get; set; }
    }
}
