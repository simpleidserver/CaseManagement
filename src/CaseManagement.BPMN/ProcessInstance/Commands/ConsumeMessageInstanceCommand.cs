using MediatR;
using Newtonsoft.Json.Linq;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class ConsumeMessageInstanceCommand : IRequest<bool>
    {
        public string FlowNodeInstanceId { get; set; }
        public string Name { get; set; }
        public JObject MessageContent { get; set; }
    }
}
