using MediatR;
using Newtonsoft.Json.Linq;

namespace CaseManagement.HumanTask.HumanTaskDef.Commands
{
    public class UpdateHumanTaskDefRenderingCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public JObject Rendering { get; set; }
    }
}
