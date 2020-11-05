using MediatR;
using System.Collections.Generic;
using static CaseManagement.HumanTask.HumanTaskDef.Results.HumanTaskDefResult;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefRenderingCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public ICollection<RenderingElementResult> RenderingElements { get; set; }
    }
}
