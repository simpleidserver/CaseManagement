using MediatR;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefRenderingCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public RenderingResult Rendering { get; set; }
    }
}
