using CaseManagement.Common.Expression;
using CaseManagement.Common.ISO8601;
using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Parser
{
    public class DeadlineParser : IDeadlineParser
    {
        public virtual ICollection<HumanTaskInstanceDeadLine> Evaluate(ICollection<HumanTaskDefinitionDeadLine> deadLines, HumanTaskInstanceDeadLineTypes type, Dictionary<string, string> parameters)
        {
            var expressionContext = new BaseExpressionContext(parameters);
            var result = new List<HumanTaskInstanceDeadLine>();
            foreach (var deadLine in deadLines)
            {
                var nextUTC = Evaluate(deadLine, expressionContext);
                if (nextUTC != null)
                {
                    result.Add(new HumanTaskInstanceDeadLine
                    {
                        EndDateTime = nextUTC.Value,
                        Name = deadLine.Name,
                        Type = type,
                        Escalations = deadLine.Escalations
                    });
                }
            }

            return result;
        }

        public virtual DateTime? Evaluate(HumanTaskDefinitionDeadLine deadLine, BaseExpressionContext expressionContext)
        {
            DateTime? nextUTC = null;
            if (!string.IsNullOrWhiteSpace(deadLine.Until))
            {
                var interval = ISO8601Parser.ParseTimeInterval(deadLine.Until);
                if (interval != null)
                {
                    nextUTC = interval.EndDateTime;
                }
            }

            if (nextUTC == null && !string.IsNullOrWhiteSpace(deadLine.For))
            {
                nextUTC = ExpressionParser.GetDateTime(deadLine.For, expressionContext);
            }

            return nextUTC;
        }
    }
}
