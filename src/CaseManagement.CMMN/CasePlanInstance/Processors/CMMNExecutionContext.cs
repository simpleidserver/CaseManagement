using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNExecutionContext : ExecutionContext<CasePlanInstanceAggregate>
    {
        public List<KeyValuePair<string, string>> IncomingTokens { get; set; }

        public CMMNExecutionContext NewExecutionContext(List<KeyValuePair<string, string>> incomingTokens)
        {
            var result = new CMMNExecutionContext
            {
                IncomingTokens = incomingTokens,
                Instance = Instance
            };
            return result;
        }
    }
}
