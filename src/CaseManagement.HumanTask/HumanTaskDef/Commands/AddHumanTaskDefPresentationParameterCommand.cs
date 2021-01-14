using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class AddHumanTaskDefPresentationParameterCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public PresentationParameterResult PresentationParameter { get; set; }
    }
}
