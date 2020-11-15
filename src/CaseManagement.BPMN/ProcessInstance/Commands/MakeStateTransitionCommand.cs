using MediatR;
using Newtonsoft.Json.Linq;

namespace CaseManagement.BPMN.ProcessInstance.Commands
{
    public class MakeStateTransitionCommand : IRequest<bool>
    {
        public string State { get; set; }
        public string FlowNodeInstanceId { get; set; }
        public string FlowNodeElementInstanceId { get; set; }
        public JObject Content { get; set; }
    }
}