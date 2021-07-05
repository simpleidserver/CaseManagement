using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.ProcessInstance.Processors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Host.Delegates
{
    public class GetWeatherInformationDelegate : IDelegateHandler
    {
        public Task<ICollection<MessageToken>> Execute(BPMNExecutionContext context, ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken)
        {
            ICollection<MessageToken> result = new List<MessageToken>();
            var rnd = new Random();
            var degree = rnd.Next(0, 40);

            result.Add(MessageToken.NewMessage(context.Pointer.InstanceFlowNodeId, "weatherInformation", new JObject
            {
                { "city", "Bruxelles" },
                { "degree", degree }
            }.ToString()));
            return Task.FromResult(result);
        }
    }
}
