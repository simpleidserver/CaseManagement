using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefParameterCommand : IRequest<string>
    {
        public string Id { get; set; }
        public ParameterResult Parameter { get; set; }
    }
}
