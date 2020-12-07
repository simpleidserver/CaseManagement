using CaseManagement.BPMN.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Acceptance.Tests.Delegates
{
    public class GetWeatherInformationDelegate : IDelegateHandler
    {
        public Task<ICollection<MessageToken>> Execute(ICollection<MessageToken> incoming, CancellationToken cancellationToken)
        {
            ICollection<MessageToken> result = new List<MessageToken>();
            result.Add(MessageToken.NewMessage("weatherInformation", new JObject
            {
                { "city", "Bruxelles" },
                { "degree", "31" }
            }.ToString()));
            return Task.FromResult(result);
        }
    }
}
