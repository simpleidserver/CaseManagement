using CaseManagement.CMMN.Domains;
using CaseManagement.Common.Processors;
using System.Collections.Generic;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CMMNExecutionContext : ExecutionContext<CasePlanInstanceAggregate>
    {
        public Dictionary<string, string> IncomingTokens { get; set; }

        public CMMNExecutionContext NewExecutionContext(Dictionary<string, string> incomingTokens)
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
