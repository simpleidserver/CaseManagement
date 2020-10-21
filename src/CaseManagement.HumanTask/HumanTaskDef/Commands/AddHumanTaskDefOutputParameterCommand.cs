using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefOutputParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public ParameterResult Parameter { get; set; }
    }
}
