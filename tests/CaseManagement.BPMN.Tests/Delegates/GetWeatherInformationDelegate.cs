using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Processors;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Tests.Delegates
{
    public class GetWeatherInformationDelegate : IDelegateHandler
    {
        public Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken)
        {
            ICollection<MessageToken> result = new List<MessageToken>();
            result.Add(MessageToken.NewMessage(context.Pointer.InstanceFlowNodeId, "weatherInformation", new JObject
            {
                { "city", "Bruxelles" },
                { "degree", "31" }
            }.ToString()));
            return Task.FromResult(result);
        }
    }
}
